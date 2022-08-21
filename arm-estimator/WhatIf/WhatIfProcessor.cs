using Azure.Core;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

internal class WhatIfProcessor
{
    private static readonly Lazy<HttpClient> httpClient = new(() => new HttpClient());
    private static readonly Dictionary<string, string> parentResourceToLocation = new();
    private static readonly Dictionary<string, RetailAPIResponse> cachedResults = new();

    private readonly ILogger logger;
    private readonly WhatIfChange[] changes;
    private readonly CurrencyCode currency;

    public WhatIfProcessor(ILogger logger, WhatIfChange[] changes, CurrencyCode currency)
    {
        this.logger = logger;
        this.changes = changes;
        this.currency = currency;
    }

    public async Task<double> Process()
    {
        double totalCost = 0;
        double alteredCost = 0;

        this.logger.LogInformation("Estimations:");
        this.logger.LogInformation("");

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

            var id = new ResourceIdentifier(change.resourceId);
            double currentChangeCost = 0;
            switch (id?.ResourceType)
            {
                case "Microsoft.Storage/storageAccounts":
                    currentChangeCost += await Calculate<StorageAccountRetailQuery, StorageAccountEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ContainerRegistry/registries":
                    currentChangeCost += await Calculate<ContainerRegistryRetailQuery, ContainerRegistryEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Web/serverfarms":
                    currentChangeCost += await Calculate<AppServicePlanRetailQuery, AppServicePlanEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Web/sites":
                    currentChangeCost += 0;
                    ReportResourceWithoutCost(id, change.changeType);
                    break;
                case "Microsoft.ContainerService/managedClusters":
                    currentChangeCost += await Calculate<AKSRetailQuery, AKSEstimationCalculation>(change, id);
                    break;
                case "Microsoft.App/containerApps":
                    currentChangeCost += await Calculate<ContainerAppsRetailQuery, ContainerAppsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Sql/servers":
                    currentChangeCost += 0;
                    ReportResourceWithoutCost(id, change.changeType);
                    break;
                case "Microsoft.Sql/servers/databases":
                    currentChangeCost += await Calculate<SQLRetailQuery, SQLEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ApiManagement/service":
                    currentChangeCost += await Calculate<APIMRetailQuery, APIMEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ApiManagement/service/gateways":
                    currentChangeCost += await Calculate<APIMRetailQuery, APIMEstimationCalculation>(change, id);
                    break;
                case "Microsoft.AppConfiguration/configurationStores":
                    currentChangeCost += await Calculate<AppConfigurationRetailQuery, AppConfigurationEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/applicationGateways":
                    currentChangeCost += await Calculate<ApplicationGatewayRetailQuery, ApplicationGatewayEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Insights/components":
                    currentChangeCost += await Calculate<ApplicationInsightsRetailQuery, ApplicationInsightsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.AnalysisServices/servers":
                    currentChangeCost += await Calculate<AnalysisServicesRetailQuery, AnalysisServicesEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/bastionHosts":
                    currentChangeCost += await Calculate<BastionRetailQuery, BastionEstimationCalculation>(change, id);
                    break;
                case "Microsoft.BotService/botServices":
                    currentChangeCost += await Calculate<BotServiceRetailQuery, BotServiceEstimationCalculation>(change, id);
                    break;
                case "Microsoft.HealthBot/healthBots":
                    currentChangeCost += await Calculate<HealthBotServiceRetailQuery, HealthBotServiceEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Chaos/experiments":
                    currentChangeCost += await Calculate<ChaosRetailQuery, ChaosEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Search/searchServices":
                    currentChangeCost += await Calculate<CognitiveSearchRetailQuery, CognitiveSearchEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ConfidentialLedger/ledgers":
                    currentChangeCost += await Calculate<ConfidentialLedgerRetailQuery, ConfidentialLedgerEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DocumentDB/databaseAccounts":
                    currentChangeCost += await Calculate<CosmosDBRetailQuery, CosmosDBEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DocumentDB/databaseAccounts/sqlDatabases":
                    currentChangeCost += await Calculate<CosmosDBRetailQuery, CosmosDBEstimationCalculation>(change, id);
                    break;
                case "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers":
                    currentChangeCost += await Calculate<CosmosDBRetailQuery, CosmosDBEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventHub/namespaces":
                    currentChangeCost += await Calculate<EventHubRetailQuery, EventHubEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventHub/namespaces/eventhubs":
                    currentChangeCost += await Calculate<EventHubRetailQuery, EventHubEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventHub/clusters":
                    currentChangeCost += await Calculate<EventHubRetailQuery, EventHubEstimationCalculation>(change, id);
                    break;
                case "Microsoft.StreamAnalytics/clusters":
                    currentChangeCost += await Calculate<StreamAnalyticsRetailQuery, StreamAnalyticsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.StreamAnalytics/streamingjobs":
                    currentChangeCost += await Calculate<StreamAnalyticsRetailQuery, StreamAnalyticsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.KeyVault/vaults":
                    currentChangeCost += await Calculate<KeyVaultRetailQuery, KeyVaultEstimationCalculation>(change, id);
                    break;
                case "Microsoft.KeyVault/managedHSMs":
                    currentChangeCost += await Calculate<KeyVaultRetailQuery, KeyVaultEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/virtualNetworkGateways":
                    currentChangeCost += await Calculate<VPNGatewayRetailQuery, VPNGatewayEstimationCalculation>(change, id);
                    break;
                case "Microsoft.SignalRService/signalR":
                    currentChangeCost += await Calculate<SignalRRetailQuery, SignalREstimationCalculation>(change, id);
                    break;
                case "Microsoft.TimeSeriesInsights/environments":
                    currentChangeCost += await Calculate<TimeSeriesRetailQuery, TimeSeriesEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Logic/workflows":
                    currentChangeCost += await Calculate<LogicAppsRetailQuery, LogicAppsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Logic/integrationAccounts":
                    currentChangeCost += await Calculate<LogicAppsRetailQuery, LogicAppsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventGrid/systemTopics":
                    currentChangeCost += await Calculate<EventGridRetailQuery, EventGridEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventGrid/topics":
                    currentChangeCost += await Calculate<EventGridRetailQuery, EventGridEstimationCalculation>(change, id);
                    break;
                case "Microsoft.EventGrid/eventSubscriptions":
                    currentChangeCost += await Calculate<EventGridRetailQuery, EventGridEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Compute/virtualMachines":
                    currentChangeCost += await Calculate<VirtualMachineRetailQuery, VirtualMachineEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/publicIPPrefixes":
                    currentChangeCost += await Calculate<PublicIPPrefixRetailQuery, PublicIPPrefixEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/publicIPAddresses":
                    currentChangeCost += await Calculate<PublicIPRetailQuery, PublicIPEstimationCalculation>(change, id);
                    break;
                case "Microsoft.OperationalInsights/workspaces":
                    currentChangeCost += await Calculate<LogAnalyticsRetailQuery, LogAnalyticsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.OperationsManagement/solutions":
                    currentChangeCost += await Calculate<LogAnalyticsRetailQuery, LogAnalyticsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Network/networkInterfaces":
                    currentChangeCost += 0;
                    ReportResourceWithoutCost(id, change.changeType);
                    break;
                case "Microsoft.Network/networkSecurityGroups":
                    currentChangeCost += 0;
                    ReportResourceWithoutCost(id, change.changeType);
                    break;
                case "Microsoft.Network/virtualNetworks":
                    currentChangeCost += 0;
                    ReportResourceWithoutCost(id, change.changeType);
                    break;
                case "Microsoft.RecoveryServices/vaults/backupPolicies":
                    currentChangeCost += 0;
                    ReportResourceWithoutCost(id, change.changeType);
                    break;
                case "Microsoft.RecoveryServices/vaults":
                    currentChangeCost += await Calculate<RecoveryServicesRetailQuery, RecoveryServicesEstimationCalculation>(change, id);
                    break;
                case "Microsoft.RecoveryServices/vaults/backupFabrics/protectionContainers/protectedItems":
                    currentChangeCost += await Calculate<RecoveryServicesProtectedItemRetailQuery, RecoveryServicesProtectedItemEstimationCalculation>(change, id);
                    break;
                default:
                    logger.LogWarning("{resourceType} is not yet supported.", id?.ResourceType);
                    break;
            }

            if (change.changeType != WhatIfChangeType.Delete)
            {
                totalCost += currentChangeCost;
            }

            if (change.changeType == WhatIfChangeType.Create)
            {
                alteredCost += currentChangeCost;
            }
            else if (change.changeType == WhatIfChangeType.Delete)
            {
                alteredCost -= currentChangeCost;
            }
        }

        var sign = "+";
        if (alteredCost < 0)
        {
            sign = "";
        }

        this.logger.LogInformation("Summary:");
        this.logger.LogInformation("");
        this.logger.AddEstimatorMessage("Total cost: {0} {1}", totalCost.ToString("N2"), this.currency);
        this.logger.AddEstimatorMessage("Delta: {0}{1} {2}", sign, alteredCost.ToString("N2"), this.currency);

        return totalCost;
    }

    private async Task<double> Calculate<TQuery, TCalculation>(WhatIfChange change, ResourceIdentifier id)
        where TQuery : BaseRetailQuery, IRetailQuery
        where TCalculation : BaseEstimation, IEstimationCalculation
    {
        var data = await GetRetailAPIResponse<TQuery>(change, id);
        if (data == null || data.Items == null)
        {
            this.logger.LogWarning("Got no records for {type} from Retail API", id.ResourceType);
            this.logger.LogInformation("");
            return 0;
        }

        if (change.resourceId == null || (change.after == null && change.before == null))
        {
            this.logger.LogError("No data available for WhatIf operation.");
            return 0;
        }

        var desiredState = change.after ?? change.before;
        if(desiredState == null)
        {
            this.logger.LogError("No data available for WhatIf operation.");
            return 0;
        }

        if (Activator.CreateInstance(typeof(TCalculation), new object[] { data.Items, id, desiredState }) is not TCalculation estimation)
        {
            this.logger.LogError("Couldn't create an instance of {type}.", typeof(TCalculation));
            return 0;
        }

        var totalCost = estimation.GetTotalCost(this.changes);

        double? delta = null;
        if(change.before != null)
        {
            if (Activator.CreateInstance(typeof(TCalculation), new object[] { data.Items, id, desiredState }) is not TCalculation previousStateEstimation)
            {
                this.logger.LogError("Couldn't create an instance of {type}.", typeof(TCalculation));
            }
            else
            {
                var previousCost = previousStateEstimation.GetTotalCost(this.changes);
                delta = totalCost - previousCost;
            }
        }

        ReportEstimationToConsole(id, estimation.GetItems(), totalCost, change.changeType, delta, data.Items?.FirstOrDefault()?.location);
        return totalCost;
    }

    private async Task<RetailAPIResponse?> GetRetailAPIResponse<T>(WhatIfChange change, ResourceIdentifier id) where T : BaseRetailQuery, IRetailQuery
    {
        var desiredState = change.after ?? change.before;
        if (desiredState == null || change.resourceId == null)
        {
            this.logger.LogError("Couldn't determine desired state for {type}.", typeof(T));
            return null;
        }

        if (desiredState.location != null)
        {
            parentResourceToLocation.Add(change.resourceId, desiredState.location);
        }

        if (Activator.CreateInstance(typeof(T), new object[] { change, id, logger, this.currency }) is not T query)
        {
            this.logger.LogError("Couldn't create an instance of {type}.", typeof(T));
            return null;
        }

        var location = desiredState.location ?? parentResourceToLocation[FindParentId(id)];
        if (location == null)
        {
            this.logger.LogError("Resources without location are not supported.");
            return null;
        }

        string? url;
        try
        {
            url = query.GetQueryUrl(location);
            if (url == null)
            {
                this.logger.LogError("URL generated for {type} is null.", typeof(T));
                return null;
            }
        }
        catch(KeyNotFoundException)
        {
            this.logger.LogWarning("{name} ({type}) [SKU is not yet supported - {sku}]", id.Name, id.ResourceType, desiredState.sku?.name);
            return null;
        }


        var data = await TryGetCachedResultForUrl(url);
        if (data == null || data.Items == null)
        {
            this.logger.LogWarning("Data for {resourceType} is not available.", id.ResourceType);
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
            this.logger.LogDebug("Getting Retail API data for {url} from cache.", url);
            data = previousResponse;
        }
        else
        {
            var response = await GetRetailDataResponse(url);
            if(response.IsSuccessStatusCode == false)
            {
                return null;
            }

            data = JsonSerializer.Deserialize<RetailAPIResponse>(await response.Content.ReadAsStreamAsync());

            if (data != null)
            {
                cachedResults.Add(urlHash, data);
            }
        }

        return data;
    }

    private string FindParentId(ResourceIdentifier id)
    {
        var currentParent = id.Parent;
        var parentType = currentParent?.Parent?.ResourceType;

        while(parentType != "Microsoft.Resources/resourceGroups" && parentType != "Microsoft.Resources/subscriptions")
        {
            currentParent = currentParent?.Parent;
            parentType = currentParent?.Parent?.ResourceType;
        }

        if(currentParent?.Name == null)
        {
            throw new Exception("Couldn't find resource parent.");
        }

        return currentParent.ToString();
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

    private void ReportEstimationToConsole(ResourceIdentifier id, IOrderedEnumerable<RetailItem> items, double? totalCost, WhatIfChangeType? changeType, double? delta, string? location)
    {
        var deltaSign = delta == null ? "+" : delta == 0 ? "" : "-";
        delta = delta == null ? totalCost : 0;

        this.logger.AddEstimatorMessageSensibleToChange(changeType, "{0}", id.Name);
        this.logger.AddEstimatorMessageSubsection("Type: {0}", id.ResourceType);
        this.logger.AddEstimatorMessageSubsection("Location: {0}", location);
        this.logger.AddEstimatorMessageSubsection("Total cost: {0} {1}", totalCost?.ToString("N2"), this.currency);
        this.logger.AddEstimatorMessageSubsection("Delta: {0} {1}", $"{deltaSign}{delta.GetValueOrDefault().ToString("N2")}", this.currency);
        this.logger.LogInformation("");
        this.logger.LogInformation("Aggregated metrics:");
        this.logger.LogInformation("");

        if (items.Any())
        {
            foreach (var item in items)
            {
                this.logger.LogInformation("-> {skuName} | {productName} | {meterName} | {retailPrice} for {measure}", item.skuName, item.productName, item.meterName, item.retailPrice, item.unitOfMeasure);
            }
        }
        else
        {
            this.logger.LogInformation("No metrics available.");
        }

        this.logger.LogInformation("");
        this.logger.LogInformation("-------------------------------");
        this.logger.LogInformation("");
    }

    private void ReportResourceWithoutCost(ResourceIdentifier id, WhatIfChangeType? changeType)
    {
        this.logger.AddEstimatorMessageSensibleToChange(changeType, "{0}", id.Name);
        this.logger.AddEstimatorMessageSubsection("Type: {0}", id.ResourceType);
        this.logger.AddEstimatorMessageSubsection("Total cost: Free");
        this.logger.LogInformation("");
        this.logger.LogInformation("-------------------------------");
        this.logger.LogInformation("");
    }
}
