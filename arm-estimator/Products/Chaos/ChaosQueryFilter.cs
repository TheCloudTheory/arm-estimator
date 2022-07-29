using Microsoft.Extensions.Logging;

internal class ChaosQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3171F2TXD";

    public ChaosQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var skuIds = ChaosSupportedData.SkuToSkuIdMap["Basic"];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
