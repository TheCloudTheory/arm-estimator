using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class StreamAnalyticsQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH316FDRQQJ";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public StreamAnalyticsQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            if(this.afterState.properties != null && this.afterState.properties.ContainsKey("sku"))
            {
                var skuData = ((JsonElement)this.afterState.properties["sku"]).Deserialize<Dictionary<string, object>>();
                if(skuData != null)
                {
                    sku = skuData["name"].ToString();
                }
            }
        }

        if(sku == null)
        {
            this.logger.LogError("Can't create a filter for Stream Analytics when SKU is unavailable.");
            return null;
        }

        var skuName = StreamAnalyticsSupportedData.SkuToSkuNameMap[sku];
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}'";
    }
}
