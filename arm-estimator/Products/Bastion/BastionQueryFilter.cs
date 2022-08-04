using Microsoft.Extensions.Logging;

internal class BastionQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3160859B7";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public BastionQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Azure Bastion when SKU is unavailable.");
            return null;
        }

        var skuIds = BastionSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
