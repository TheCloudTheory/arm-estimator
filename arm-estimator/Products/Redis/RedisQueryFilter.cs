using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class RedisQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH314LP4PZS";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public RedisQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        if (this.afterState.properties?.ContainsKey("sku") == null)
        {
            this.logger.LogError("Can't create a filter for Redis when SKU is unavailable.");
            return null;
        }

        var skuData = ((JsonElement)this.afterState.properties["sku"]).Deserialize<RedisSkuData>();
        if(skuData == null)
        {
            this.logger.LogError("Can't create a filter for Redis when SKU data cannot be obtained.");
            return null;
        }

        var family = skuData.family;
        var capacity = skuData.capacity;
        var skuName = $"{family}{capacity}";

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq 'Azure Redis Cache {skuData.name}' and meterName eq '{skuName} Cache'";
    }
}
