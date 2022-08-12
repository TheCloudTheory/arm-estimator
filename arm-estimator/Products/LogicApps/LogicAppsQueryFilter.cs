using Microsoft.Extensions.Logging;

internal class LogicAppsQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH319T30TWX";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public LogicAppsQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            sku = "Consumption";
        }

        var skuIds = LogicAppsSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
