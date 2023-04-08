using ACE.Calculation;
using ACE.Extensions;
using ACE.Output;
using ACE.ResourceManager;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ACE.WhatIf;

internal class WhatIfProcessor
{
    private static readonly Lazy<HttpClient> httpClient = new(() => new HttpClient());
    private static readonly ConcurrentDictionary<string, RetailAPIResponse> cachedResults = new();
    private static readonly Dictionary<string, string> resourceIdToLocationMap = new();

    private readonly ILogger logger;
    private readonly WhatIfChange[] changes;
    private readonly CurrencyCode currency;
    private readonly TemplateSchema? template;
    private readonly IOutputFormatter outputFormatter;

    public WhatIfProcessor(ILogger logger,
                           WhatIfChange[] changes,
                           CurrencyCode currency,
                           bool disableDetailedMetrics,
                           TemplateSchema? template,
                           OutputFormat outputFormat)
    {
        this.logger = logger;
        this.changes = changes;
        this.currency = currency;
        this.template = template;
        this.outputFormatter = new OutputGenerator(outputFormat, logger, currency, disableDetailedMetrics).GetFormatter();

        ReconcileResources();
        BuildVMCapabilitiesIfNeeded();
    }

    /// <summary>
    /// This method maps resources to their locations. As it's easier said than done,
    /// short clarification.
    /// 
    /// Some resources aren't defining location on their own. Instead, they rely on location
    /// provided by their parent. This is exactly what it's being done here - we iterate
    /// over detected changes and try to find a parent going up by the hierarchy. Note,
    /// that this method is not ideal - if template contains a child resource only, it'll
    /// assume location of the resource group.
    /// </summary>
    private void ReconcileResources()
    {
        foreach (var change in this.changes)
        {
            var actualChange = change.GetChange();
            if (actualChange == null || change.resourceId == null)
            {
                this.logger.LogWarning("[Reconcile] Skipping resource because it's missing required data.");
                continue;
            }

            if(actualChange.location != null)
            {
                if(resourceIdToLocationMap.ContainsKey(change.resourceId) == false)
                {
                    resourceIdToLocationMap.Add(change.resourceId, actualChange.location);
                }
                
                continue;
            }

            string? location = null;
            var resourceId = change.resourceId;
            while (location == null)
            {
                var id = new CommonResourceIdentifier(resourceId);
                var parent = id.GetParent();

                if(parent == null)
                {
                    this.logger.LogWarning("[Reconcile] Skipping resource because it doesn't have a parent to get a location from.");
                    break;
                }

                var parentChange = this.changes.SingleOrDefault(_ => _.resourceId == parent.ToString());
                if (parentChange == null)
                {
                    resourceId = parent.ToString();
                    continue;
                }

                actualChange = parentChange.GetChange();
                if (actualChange != null && actualChange.location != null)
                {
                    location = actualChange.location;
                    resourceIdToLocationMap.Add(change.resourceId, location);
                    break;
                }

                resourceId = parent.ToString();
            }
        }
    }

    private void BuildVMCapabilitiesIfNeeded()
    {
        var vmChanges = this.changes.Where(_ => new CommonResourceIdentifier(_.resourceId!).GetResourceType() == "Microsoft.Compute/virtualMachines");
        if(vmChanges != null && vmChanges.Any())
        {
            this.logger.AddEstimatorMessage("Changes contain VM resource - attempting to build capabilities cache.");

            var location = vmChanges.First().GetChange()?.location;
            if(location == null)
            {
                this.logger.LogError("Couldn't find location to build VM capabilities cache.");
                return;
            }

            CapabilitiesCache.InitializeVirtualMachineCapabilities(location);

            this.logger.AddEstimatorMessage("Capabilities cache initialized.");
            logger.LogInformation("");
            logger.LogInformation("-------------------------------");
            logger.LogInformation("");
        }
    }

    public async Task<EstimationOutput> Process()
    {
        double totalCost = 0;
        double delta = 0;

        this.outputFormatter.BeginEstimationsBlock();

        var resources = new List<EstimatedResourceData>();
        var unsupportedResources = new List<CommonResourceIdentifier>();
        var freeResources = new Dictionary<CommonResourceIdentifier, WhatIfChangeType?>();

        foreach (WhatIfChange change in this.changes)
        {
            if (change.resourceId == null)
            {
                logger.LogWarning("Ignoring resource with empty resource ID");
                continue;
            }

            if (change.after == null && change.before == null)
            {
                logger.LogWarning("Ignoring resource with empty desired state.");
                continue;
            }

            var id = new CommonResourceIdentifier(change.resourceId);
            EstimatedResourceData? resource = null;
            switch (id?.GetResourceType())
            {
                case "Microsoft.Storage/storageAccounts":
                    resource = await Calculate<StorageAccountRetailQuery, StorageAccountEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ContainerRegistry/registries":
                    resource = await Calculate<ContainerRegistryRetailQuery, ContainerRegistryEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Web/serverfarms":
                    resource = await Calculate<AppServicePlanRetailQuery, AppServicePlanEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Web/sites":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.ContainerService/managedClusters":
                    resource = await Calculate<AKSRetailQuery, AKSEstimationCalculation>(change, id);
                    break;
                case "Microsoft.App/containerApps":
                    resource = await Calculate<ContainerAppsRetailQuery, ContainerAppsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Sql/servers":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Sql/servers/databases":
                    resource = await Calculate<SQLRetailQuery, SQLEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ApiManagement/service":
                    resource = await Calculate<APIMRetailQuery, APIMEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ApiManagement/service/gateways":
                    resource = await Calculate<APIMRetailQuery, APIMEstimationCalculation>(change, id);
                    break;
                case "Microsoft.AppConfiguration/configurationStores":
                    resource = await Calculate<AppConfigurationRetailQuery, AppConfigurationEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/applicationGateways":
                    resource = await Calculate<ApplicationGatewayRetailQuery, ApplicationGatewayEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Insights/components":
                    resource = await Calculate<ApplicationInsightsRetailQuery, ApplicationInsightsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.AnalysisServices/servers":
                    resource = await Calculate<AnalysisServicesRetailQuery, AnalysisServicesEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/bastionHosts":
                    resource = await Calculate<BastionRetailQuery, BastionEstimationCalculation>(change, id);
                    break;
                case "Microsoft.BotService/botServices":
                    resource = await Calculate<BotServiceRetailQuery, BotServiceEstimationCalculation>(change, id);
                    break;
                case "Microsoft.HealthBot/healthBots":
                    resource = await Calculate<HealthBotServiceRetailQuery, HealthBotServiceEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Chaos/experiments":
                    resource = await Calculate<ChaosRetailQuery, ChaosEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Search/searchServices":
                    resource = await Calculate<CognitiveSearchRetailQuery, CognitiveSearchEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ConfidentialLedger/ledgers":
                    resource = await Calculate<ConfidentialLedgerRetailQuery, ConfidentialLedgerEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DocumentDB/databaseAccounts":
                    resource = await Calculate<CosmosDBRetailQuery, CosmosDBEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DocumentDB/databaseAccounts/sqlDatabases":
                    resource = await Calculate<CosmosDBRetailQuery, CosmosDBEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers":
                    resource = await Calculate<CosmosDBRetailQuery, CosmosDBEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventHub/namespaces":
                    resource = await Calculate<EventHubRetailQuery, EventHubEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventHub/namespaces/eventhubs":
                    resource = await Calculate<EventHubRetailQuery, EventHubEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventHub/clusters":
                    resource = await Calculate<EventHubRetailQuery, EventHubEstimationCalculation>(change, id);
                    break;
                case "Microsoft.StreamAnalytics/clusters":
                    resource = await Calculate<StreamAnalyticsRetailQuery, StreamAnalyticsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.StreamAnalytics/streamingjobs":
                    resource = await Calculate<StreamAnalyticsRetailQuery, StreamAnalyticsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.KeyVault/vaults":
                    resource = await Calculate<KeyVaultRetailQuery, KeyVaultEstimationCalculation>(change, id);
                    break;
                case "Microsoft.KeyVault/managedHSMs":
                    resource = await Calculate<KeyVaultRetailQuery, KeyVaultEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/virtualNetworkGateways":
                    resource = await Calculate<VPNGatewayRetailQuery, VPNGatewayEstimationCalculation>(change, id);
                    break;
                case "Microsoft.SignalRService/signalR":
                    resource = await Calculate<SignalRRetailQuery, SignalREstimationCalculation>(change, id);
                    break;
                case "Microsoft.TimeSeriesInsights/environments":
                    resource = await Calculate<TimeSeriesRetailQuery, TimeSeriesEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Logic/workflows":
                    resource = await Calculate<LogicAppsRetailQuery, LogicAppsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Logic/integrationAccounts":
                    resource = await Calculate<LogicAppsRetailQuery, LogicAppsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventGrid/systemTopics":
                    resource = await Calculate<EventGridRetailQuery, EventGridEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventGrid/topics":
                    resource = await Calculate<EventGridRetailQuery, EventGridEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventGrid/eventSubscriptions":
                    resource = await Calculate<EventGridRetailQuery, EventGridEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Compute/virtualMachines":
                    resource = await Calculate<VirtualMachineRetailQuery, VirtualMachineEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/publicIPPrefixes":
                    resource = await Calculate<PublicIPPrefixRetailQuery, PublicIPPrefixEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/publicIPAddresses":
                    resource = await Calculate<PublicIPRetailQuery, PublicIPEstimationCalculation>(change, id);
                    break;
                case "Microsoft.OperationalInsights/workspaces":
                    resource = await Calculate<LogAnalyticsRetailQuery, LogAnalyticsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.OperationsManagement/solutions":
                    resource = await Calculate<LogAnalyticsRetailQuery, LogAnalyticsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/networkInterfaces":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Network/networkSecurityGroups":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Network/virtualNetworks":
                    resource = await Calculate<VNetRetailQuery, VNetEstimationCalculation>(change, id, true);
                    break;
                case "Microsoft.RecoveryServices/vaults/backupPolicies":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.RecoveryServices/vaults":
                    resource = await Calculate<RecoveryServicesRetailQuery, RecoveryServicesEstimationCalculation>(change, id);
                    break;
                case "Microsoft.RecoveryServices/vaults/backupFabrics/protectionContainers/protectedItems":
                    resource = await Calculate<RecoveryServicesProtectedItemRetailQuery, RecoveryServicesProtectedItemEstimationCalculation>(change, id);
                    break;
                case "Microsoft.RecoveryServices/vaults/replicationFabrics":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.RecoveryServices/vaults/replicationFabrics/replicationNetworks/replicationNetworkMappings":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers/replicationProtectionContainerMappings":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers/replicationProtectedItems":
                    resource = await Calculate<AzureSiteRecoveryRetailQuery, AzureSiteRecoveryEstimationCalculation>(change, id);
                    break;
                case "Microsoft.RecoveryServices/vaults/replicationPolicies":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.RecoveryServices/vaults/backupstorageconfig":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Insights/metricAlerts":
                    resource = await Calculate<MonitorRetailQuery, MonitorEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Insights/scheduledQueryRules":
                    resource = await Calculate<MonitorRetailQuery, MonitorEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DBforMariaDB/servers":
                    resource = await Calculate<MariaDBRetailQuery, MariaDBEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DBforMariaDB/servers/virtualNetworkRules":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Cache/redis":
                    resource = await Calculate<RedisRetailQuery, RedisEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/ipGroups":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Network/firewallPolicies":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Network/firewallPolicies/ruleCollectionGroups":
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Network/azureFirewalls":
                    resource = await Calculate<FirewallRetailQuery, FirewallEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Storage/storageAccounts/blobServices":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Storage/storageAccounts/blobServices/containers":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Automation/automationAccounts":
                    resource = await Calculate<AutomationAccountRetailQuery, AutomationAccountEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Automation/automationAccounts/runbooks":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.OperationalInsights/workspaces/linkedServices":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.DBforPostgreSQL/servers":
                    resource = await Calculate<PostgreSQLRetailQuery, PostgreSQLEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DBforPostgreSQL/flexibleServers":
                    resource = await Calculate<PostgreSQLFlexibleRetailQuery, PostgreSQLFlexibleEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ServiceBus/namespaces":
                    resource = await Calculate<ServiceBusRetailQuery, ServiceBusEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DataFactory/factories/datasets":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.DataFactory/factories/pipelines":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.DataFactory/factories/linkedservices":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.DataFactory/factories":
                    resource = await Calculate<DataFactoryRetailQuery, DataFactoryEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Compute/availabilitySets":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Compute/virtualMachineScaleSets":
                    resource = await Calculate<VmssRetailQuery, VmssEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Authorization/policyAssignments":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Authorization/roleAssignments":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Insights/diagnosticSettings":
                    resource = await Calculate<DiagnosticSettingsRetailQuery, DiagnosticSettingsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ManagedIdentity/userAssignedIdentities":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                case "Microsoft.Cache/redisEnterprise":
                    resource = await Calculate<RedisEnterpriseRetailQuery, RedisEnterpriseEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/virtualNetworks/subnets":
                    resource = new EstimatedResourceData(0, 0, id);
                    freeResources.Add(id, change.changeType);
                    break;
                default:
                    if (id?.GetName() != null)
                    {
                        unsupportedResources.Add(id);
                    }

                    break;
            }

            if (resource == null) continue;
            if (change.changeType != WhatIfChangeType.Delete)
            {
                totalCost += resource.TotalCost.OriginalValue;
            }

            if (change.changeType == WhatIfChangeType.Create)
            {
                delta += resource.TotalCost.OriginalValue;
            }
            else if (change.changeType == WhatIfChangeType.Delete)
            {
                delta -= resource.TotalCost.OriginalValue;
            }

            resources.Add(resource);
        }

        var sign = "+";
        if (delta < 0)
        {
            sign = "";
        }

        if (resources.Count == 0)
        {
            logger.AddEstimatorMessage("No resource available for estimation.");
            logger.LogInformation("");
            logger.LogInformation("-------------------------------");
            logger.LogInformation("");
        }

        this.outputFormatter.EndEstimationsBlock();

        if (freeResources.Count > 0)
        {
            this.outputFormatter.RenderFreeResourcesBlock(freeResources);         
        }

        if (unsupportedResources.Count > 0)
        {
            this.outputFormatter.RenderUnsupportedResourcesBlock(unsupportedResources);
        }

        logger.LogInformation("Summary:");
        logger.LogInformation("");
        logger.AddEstimatorMessage("Total cost: {0} {1}", totalCost.ToString("N2"), currency);
        logger.AddEstimatorMessage("Delta: {0}{1} {2}", sign, delta.ToString("N2"), currency);
        logger.LogInformation("");

        return new EstimationOutput(totalCost, delta, resources, currency, changes.Length, unsupportedResources.Count);
    }

    private async Task<EstimatedResourceData?> Calculate<TQuery, TCalculation>(WhatIfChange change, CommonResourceIdentifier id, bool? useFakeApiResponse = null)
        where TQuery : BaseRetailQuery, IRetailQuery
        where TCalculation : BaseEstimation, IEstimationCalculation
    {
        var data = useFakeApiResponse == null || useFakeApiResponse.Value == false ?
            await GetRetailAPIResponse<TQuery>(change, id) :
            GetFakeRetailAPIResponse<TQuery>(change, id);

        if (data == null || data.Items == null)
        {
            logger.LogWarning("Got no records for {type} from Retail API", id.GetResourceType());
            logger.LogInformation("");

            return null;
        }

        if (change.resourceId == null || change.after == null && change.before == null)
        {
            logger.LogError("No data available for WhatIf operation.");
            return null;
        }

        var desiredState = change.after ?? change.before;
        if (desiredState == null)
        {
            logger.LogError("No data available for WhatIf operation.");
            return null;
        }

        if (Activator.CreateInstance(typeof(TCalculation), new object[] { data.Items, id, desiredState }) is not TCalculation estimation)
        {
            logger.LogError("Couldn't create an instance of {type}.", typeof(TCalculation));
            return null;
        }

        var summary = estimation.GetTotalCost(changes, template?.Metadata?.UsagePatterns);

        double? delta = null;
        if (change.before != null)
        {
            if (Activator.CreateInstance(typeof(TCalculation), new object[] { data.Items, id, desiredState }) is not TCalculation previousStateEstimation)
            {
                logger.LogError("Couldn't create an instance of {type}.", typeof(TCalculation));
            }
            else
            {
                var previousSummary = previousStateEstimation.GetTotalCost(changes, template?.Metadata?.UsagePatterns);
                delta = summary.TotalCost - previousSummary.TotalCost;
            }
        }

        this.outputFormatter.ReportEstimationToConsole(id, estimation.GetItems(), summary, change.changeType, delta, data.Items?.FirstOrDefault()?.location);
        return new EstimatedResourceData(summary.TotalCost, delta, id);
    }

    private async Task<RetailAPIResponse?> GetRetailAPIResponse<T>(WhatIfChange change, CommonResourceIdentifier id) where T : BaseRetailQuery, IRetailQuery
    {
        var desiredState = change.after ?? change.before;
        if (desiredState == null || change.resourceId == null)
        {
            logger.LogError("Couldn't determine desired state for {type}.", typeof(T));
            return null;
        }

        if (Activator.CreateInstance(typeof(T), new object[] { change, id, logger, currency, changes }) is not T query)
        {
            logger.LogError("Couldn't create an instance of {type}.", typeof(T));
            return null;
        }

        var location = resourceIdToLocationMap[id.ToString()];
        if (location == null)
        {
            logger.LogError("Resources without location are not supported.");
            return null;
        }

        string? url;
        try
        {
            url = query.GetQueryUrl(location);
            if (url == null)
            {
                logger.LogError("URL generated for {type} is null.", typeof(T));
                return null;
            }
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("{name} ({type}) [SKU is not yet supported - {sku}]", id.GetName(), id.GetResourceType(), desiredState.sku?.name);
            return null;
        }


        var data = await TryGetCachedResultForUrl(url);
        if (data == null || data.Items == null)
        {
            logger.LogWarning("Data for {resourceType} is not available.", id.GetResourceType());
            return null;
        }

        return data;
    }

    private async Task<RetailAPIResponse?> TryGetCachedResultForUrl(string url)
    {
        RetailAPIResponse? data;
        var urlHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(url));
        if (cachedResults.TryGetValue(urlHash, out var previousResponse))
        {
            logger.LogDebug("Getting Retail API data for {url} from cache.", url);
            data = previousResponse;
        }
        else
        {
            if (url == "SKIP")
            {
                data = new RetailAPIResponse()
                {
                    Items = Enumerable.Empty<RetailItem>().ToArray()
                };

                return data;
            }

            var response = await GetRetailDataResponse(url);
            if (response.IsSuccessStatusCode == false)
            {
                return null;
            }

            data = JsonSerializer.Deserialize<RetailAPIResponse>(await response.Content.ReadAsStreamAsync());

            if (data != null)
            {
                data.Items = data.Items?.Distinct(new RetailAPIEqualityComparer()).ToArray();
                cachedResults.GetOrAdd(urlHash, data);
            }
        }

        return data;
    }

    private RetailAPIResponse? GetFakeRetailAPIResponse<T>(WhatIfChange change, CommonResourceIdentifier id) where T : BaseRetailQuery, IRetailQuery
    {
        if (Activator.CreateInstance(typeof(T), new object[] { change, id, logger, currency, changes }) is not T query)
        {
            logger.LogError("Couldn't create an instance of {type}.", typeof(T));
            return null;
        }

        return query.GetFakeResponse();
    }

    private async Task<HttpResponseMessage> GetRetailDataResponse(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await httpClient.Value.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return response;
        }
        else
        {
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}