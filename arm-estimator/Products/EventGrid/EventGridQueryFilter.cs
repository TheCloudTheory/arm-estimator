using Microsoft.Extensions.Logging;

internal class EventGridQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH315NX4WLD";

    public EventGridQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = "Standard";
        var skuIds = EventGridSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
