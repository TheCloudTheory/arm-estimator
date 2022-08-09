using Azure.Core;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

internal class WhatIfProcessor
{
    private static readonly Lazy<HttpClient> httpClient = new(() => new HttpClient());
    private static readonly Dictionary<string, string> parentResourceToLocation = new();
    private static readonly Dictionary<string, RetailAPIResponse> cachedResults = new();

    private readonly ILogger logger;
    private readonly WhatIfChange[] changes;

    public WhatIfProcessor(ILogger logger, WhatIfChange[] changes)
    {
        this.logger = logger;
        this.changes = changes;
    }

    public async Task<double> Process()
    {
        double totalCost = 0;
        double alteredCost = 0;

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
                    break;
                case "Microsoft.ContainerService/managedClusters":
                    currentChangeCost += await Calculate<AKSRetailQuery, AKSEstimationCalculation>(change, id);
                    break;
                case "Microsoft.App/containerApps":
                    currentChangeCost += await Calculate<ContainerAppsRetailQuery, ContainerAppsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Sql/servers":
                    currentChangeCost += 0;
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

        this.logger.LogError("Total cost: {cost} USD", totalCost);
        this.logger.LogError("Delta: {sign}{cost} USD", sign, alteredCost);

        return totalCost;
    }

    private async Task<double> Calculate<TQuery, TCalculation>(WhatIfChange change, ResourceIdentifier id)
        where TQuery : BaseRetailQuery, IRetailQuery
        where TCalculation : BaseEstimation, IEstimationCalculation
    {
        var data = await GetAPIResponse<TQuery>(change, id);
        if (data == null || data.Items == null)
        {
            this.logger.LogWarning("Got no records for {type} from Retail API", id.ResourceType);
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
        ReportEstimationToConsole(id, estimation.GetItems(), totalCost);

        return totalCost;
    }

    private async Task<RetailAPIResponse?> GetAPIResponse<T>(WhatIfChange change, ResourceIdentifier id) where T : BaseRetailQuery, IRetailQuery
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

        if (Activator.CreateInstance(typeof(T), new object[] { change, id, logger }) is not T query)
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

        var url = query.GetQueryUrl(location);
        if (url == null)
        {
            this.logger.LogError("URL generated for {type} is null.", typeof(T));
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

        while(parentType != "Microsoft.Resources/resourceGroups")
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

        response.EnsureSuccessStatusCode();
        return response;
    }

    private void ReportEstimationToConsole(ResourceIdentifier id, IOrderedEnumerable<RetailItem> items, double? totalCost)
    {
        this.logger.LogInformation("Price for {name} [{resourceType}] will be {totalCost} USD.", id.Name, id.ResourceType, totalCost);
        this.logger.LogInformation("");
        this.logger.LogInformation("-> Instance: {name}", id.Name);
        this.logger.LogInformation("-> Type: {type}", id.ResourceType);
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
}
