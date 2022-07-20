internal class StorageAccountQueryFilter
{
    private const string ServiceId = "DZH317F1HKN0";

    private readonly WhatIfAfterChange afterState;

    public StorageAccountQueryFilter(WhatIfAfterChange afterState)
    {
        this.afterState = afterState;
    }

    public string GetFiltersBasedOnDesiredState()
    {
        var location = this.afterState.location;
        var sku = this.afterState.sku?.name;
        var skuIds = StorageAccountSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
