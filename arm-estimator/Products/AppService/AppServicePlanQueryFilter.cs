using Microsoft.Extensions.Logging;

internal class AppServicePlanQueryFilter : IQueryFilter
{
    private readonly WhatIfAfterChange afterState;
    private readonly ILogger logger;

    public AppServicePlanQueryFilter(WhatIfAfterChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState()
    {
        var location = this.afterState.location;
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for App Service Plan when SKU is unavailable.");
            return null;
        }

        var serviceId = AppServicePlanSupportedData.SkuToServiceId[sku];
        var skuIds = AppServicePlanSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{serviceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
