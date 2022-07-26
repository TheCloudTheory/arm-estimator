using Microsoft.Extensions.Logging;

internal class APIMQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH318FZ20SD";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public APIMQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        var type = this.afterState.type;

        if(type != null && type == "Microsoft.ApiManagement/service/gateways")
        {
            sku = "Gateway";
        }

        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for API Management Service when SKU is unavailable.");
            return null;
        }

        var skuIds = APIMSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
