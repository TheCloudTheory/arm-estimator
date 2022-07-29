using Microsoft.Extensions.Logging;

internal class HealthBotServiceQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH318J9W0D1";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public HealthBotServiceQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
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

        var skuIds = HealthBotServiceSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
