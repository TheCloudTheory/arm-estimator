using Microsoft.Extensions.Logging;

internal class SignalRQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH319BFCV72";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public SignalRQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for SignalR when SKU is unavailable.");
            return null;
        }

        if(sku == "Free_F1")
        {
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and meterId eq '799e5620-1547-45f2-96fe-8df86291fd7c'";
        }

        var skuIds = SignalRSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
