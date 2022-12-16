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

    public string? GetFiltersBasedOnDesiredState(string location)
    {
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

        if(tier == "Free")
        {
            return "SKIP";
        }

        var skuIds = AKSSupportedData.TierToSkuNameMap[tier];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuName eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
