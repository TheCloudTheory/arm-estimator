using Microsoft.Extensions.Logging;

internal class AppConfigurationQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH319K8RFPB";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public AppConfigurationQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for App Configuration when SKU is unavailable.");
            return null;
        }

        var skuIds = AppConfigurationSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
