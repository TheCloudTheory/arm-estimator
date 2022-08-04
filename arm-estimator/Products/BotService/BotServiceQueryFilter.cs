using Microsoft.Extensions.Logging;

internal class BotServiceQueryFilter : IQueryFilter
{
    private const string ServiceId = "BOTSERVICE";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public BotServiceQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Bot Service when SKU is unavailable.");
            return null;
        }

        var skuIds = BotServiceSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
