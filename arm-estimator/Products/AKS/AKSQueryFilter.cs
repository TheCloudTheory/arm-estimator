using Microsoft.Extensions.Logging;

internal class AKSQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH315HTQSK6";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public AKSQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState()
    {
        var location = this.afterState.location;
        var sku = this.afterState.sku;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Azure Kubernetes Service when SKU is unavailable.");
            return null;
        }

        var tier = sku.tier;
        if(tier == null)
        {
            tier = "Free";
        }

        var skuIds = AKSSupportedData.TierToSkuIdMap[tier];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
