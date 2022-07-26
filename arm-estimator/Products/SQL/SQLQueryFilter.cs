using Microsoft.Extensions.Logging;

internal class SQLQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3180HX10K";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public SQLQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Azure SQL when SKU is unavailable.");
            return null;
        }

        var skuIds = SQLSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
