using Microsoft.Extensions.Logging;

internal class ContainerRegistryQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH315F9L8DM";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public ContainerRegistryQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Storage Account when SKU is unavailable.");
            return null;
        }

        var skuIds = ContainerRegistrySupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
