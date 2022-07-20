using Azure.Core;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class WhatIfProcessor
{
    private static readonly Lazy<HttpClient> httpClient = new(() => new HttpClient());

    public static async Task Process(WhatIfChange[] changes, ILogger logger)
    {
        foreach (WhatIfChange change in changes)
        {
            if (change.resourceId == null || change.after == null || change.after.location == null)
            {
                logger.LogWarning("Ignoring resource with empty resource ID or location.");
                continue;
            }

            var id = new ResourceIdentifier(change.resourceId);
            string? url;
            switch (id.ResourceType)
            {
                case "Microsoft.Storage/storageAccounts":
                    var query = new StorageAccountRetailQuery(change, logger);
                    url = query.GetQueryUrl();

                    break;
                default:
                    logger.LogWarning("{resourceType} is not yet supported.", id.ResourceType);
                    url = null;
                    break;
            }

            if(url == null)
            {
                continue;
            }

            var response = await GetRetailDataResponse(url);
            var data = JsonSerializer.Deserialize<RetailAPIResponse>(await response.Content.ReadAsStreamAsync());

            if (data == null || data.Items == null)
            {
                logger.LogWarning("Data for {resourceType} is not available.", id.ResourceType);
                continue;
            }

            var totalCost = data.Items.Where(_ => _.type != "Reservation").Select(_ => _.retailPrice).Sum();

            logger.LogInformation("Price for {name} [{resourceType}] will be {totalCost} USD.", id.Name, id.ResourceType, totalCost);
        }
    }

    private static async Task<HttpResponseMessage> GetRetailDataResponse(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await httpClient.Value.SendAsync(request);

        response.EnsureSuccessStatusCode();
        return response;
    }
}
