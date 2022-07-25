﻿using Azure.Core;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class WhatIfProcessor
{
    private static readonly Lazy<HttpClient> httpClient = new(() => new HttpClient());
    private readonly ILogger logger;

    public WhatIfProcessor(ILogger logger)
    {
        this.logger = logger;
    }

    public async Task Process(WhatIfChange[] changes)
    {
        double totalCost = 0;

        foreach (WhatIfChange change in changes)
        {
            if (change.resourceId == null || change.after == null || change.after.location == null)
            {
                logger.LogWarning("Ignoring resource with empty resource ID or location.");
                continue;
            }

            var id = new ResourceIdentifier(change.resourceId);
            if (change.changeType == WhatIfChangeType.NoChange)
            {
                logger.LogInformation("Ignoring {name} [{type}] - no change detected.", id.Name, id.ResourceType);
                continue;
            }

            switch (id.ResourceType)
            {
                case "Microsoft.Storage/storageAccounts":
                    totalCost += await Calculate<StorageAccountRetailQuery, StorageAccountEstimationCalculation>(change, id);
                    break;
                case "Microsoft.ContainerRegistry/registries":
                    totalCost += await Calculate<ContainerRegistryRetailQuery, ContainerRegistryEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Web/serverfarms":
                    totalCost += await Calculate<AppServicePlanRetailQuery, AppServicePlanEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Web/sites":
                    totalCost += 0;
                    break;
                case "Microsoft.ContainerService/managedClusters":
                    totalCost += await Calculate<AKSRetailQuery, AKSEstimationCalculation>(change, id);
                    break;
                case "Microsoft.App/containerApps":
                    totalCost += await Calculate<ContainerAppsRetailQuery, ContainerAppsEstimationCalculation>(change, id);
                    break;
                case "Microsoft.Sql/servers":
                    totalCost += 0;
                    break;
                case "Microsoft.Sql/servers/databases":
                    totalCost += await Calculate<SQLRetailQuery, SQLEstimationCalculation>(change, id);
                    break;
                default:
                    logger.LogWarning("{resourceType} is not yet supported.", id.ResourceType);
                    break;
            }
        }

        this.logger.LogError("Total cost: {cost} USD", totalCost);
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

        if(change.after == null)
        {
            this.logger.LogError("No data available for WhatIf operation.");
            return 0;
        }

        if (Activator.CreateInstance(typeof(TCalculation), new object[] { data.Items, change.after }) is not TCalculation estimation)
        {
            this.logger.LogError("Couldn't create an instance of {type}.", typeof(TCalculation));
            return 0;
        }

        var totalCost = estimation.GetTotalCost();
        ReportEstimationToConsole(id, estimation.GetItems(), totalCost);

        return totalCost;
    }

    private async Task<RetailAPIResponse?> GetAPIResponse<T>(WhatIfChange change, ResourceIdentifier id) where T : BaseRetailQuery, IRetailQuery
    {
        if (Activator.CreateInstance(typeof(T), new object[] { change, logger }) is not T saQuery)
        {
            this.logger.LogError("Couldn't create an instance of {type}.", typeof(T));
            return null;
        }

        var url = saQuery.GetQueryUrl();
        if(url == null)
        {
            this.logger.LogError("URL generated for {type} is null.", typeof(T));
            return null;
        }

        var response = await GetRetailDataResponse(url);
        var data = JsonSerializer.Deserialize<RetailAPIResponse>(await response.Content.ReadAsStreamAsync());

        if (data == null || data.Items == null)
        {
            this.logger.LogWarning("Data for {resourceType} is not available.", id.ResourceType);
            return null;
        }

        return data;
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

        if(items.Any())
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