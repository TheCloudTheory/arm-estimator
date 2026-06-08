using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class RedisEnterpriseQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH314LP4PZS";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public RedisEnterpriseQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        if (this.afterState.properties?.ContainsKey("sku") == null)
        {
            this.logger.LogError("Can't create a filter for Redis Enterprise when SKU is unavailable.");
            return null;
        }

        var skuData = this.afterState.sku;
        if(skuData == null)
        {
            this.logger.LogError("Can't create a filter for Enterprise when SKU data cannot be obtained.");
            return null;
        }

        if(skuData.name == null)
        {
            this.logger.LogError("Can't create a filter for Enterprise when SKU name is null.");
            return null;
        }

        var skuName = skuData.name.Split("_")[1];
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}'";
    }
}
