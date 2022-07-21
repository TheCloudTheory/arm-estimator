using Azure.Core;
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
        foreach (WhatIfChange change in changes)
        {
            if (change.resourceId == null || change.after == null || change.after.location == null)
            {
                logger.LogWarning("Ignoring resource with empty resource ID or location.");
                continue;
            }

            var id = new ResourceIdentifier(change.resourceId);
            switch (id.ResourceType)
            {
                case "Microsoft.Storage/storageAccounts":
                    await CalculateForStorageAccount(change, id);
                    break;
                case "Microsoft.ContainerRegistry/registries":
                    await CalculateForContainerRegistry(change, id);
                    break;
                default:
                    logger.LogWarning("{resourceType} is not yet supported.", id.ResourceType);
                    break;
            }
        }
    }

    private async Task CalculateForStorageAccount(WhatIfChange change, ResourceIdentifier id)
    {
        var data = await GetAPIResponse<ContainerRegistryRetailQuery>(change, id);
        if(data == null || data.Items == null)
        {
            this.logger.LogWarning("Got no records for {type} from Retail API", id.ResourceType);
            return;
        }

        var itemsWithoutReservations = data.Items.Where(_ => _.type != "Reservation").OrderByDescending(_ => _.retailPrice);
        var totalCost = itemsWithoutReservations.Select(_ => _.retailPrice).Sum();

        ReportEstimationToConsole(id, itemsWithoutReservations, totalCost);
    }

    private async Task CalculateForContainerRegistry(WhatIfChange change, ResourceIdentifier id)
    {
        var data = await GetAPIResponse<ContainerRegistryRetailQuery>(change, id);
        if (data == null || data.Items == null)
        {
            this.logger.LogWarning("Got no records for {type} from Retail API", id.ResourceType);
            return;
        }

        var estimation = new ContainerRegistryEstimationCalculation(data.Items);
        ReportEstimationToConsole(id, estimation.GetItems(), estimation.GetTotalCost());
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
            this.logger.LogError("URL generatet for {type} is null.", typeof(T));
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
        this.logger.LogInformation("----------------------");
        this.logger.LogInformation("Instance: {name}", id.Name);
        this.logger.LogInformation("Type: {type}", id.ResourceType);

        foreach (var item in items)
        {
            this.logger.LogInformation("{skuName} | {productName} | {meterName} | {retailPrice} for {measure}", item.skuName, item.productName, item.meterName, item.retailPrice, item.unitOfMeasure);
        }
    }
}
