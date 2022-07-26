using Microsoft.Extensions.Logging;

internal class StorageAccountQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH317F1HKN0";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public StorageAccountQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState()
    {
        var location = this.afterState.location;
        var sku = this.afterState.sku?.name;
        var kind = this.afterState.kind;

        if(sku == null)
        {
            this.logger.LogError("Can't create a filter for Storage Account when SKU is unavailable.");
            return null;
        }

        if(kind == null)
        {
            this.logger.LogError("Can't create a filter for Storage Account when Kind is unavailable.");
            return null;
        }

        var skuParts = sku.Split("_");
        var accountTier = skuParts[0];
        var replicationType = skuParts[1];
        var blobAccessTier = "Hot";

        if(this.afterState.properties != null && this.afterState.properties.ContainsKey("accessTier"))
        {
            blobAccessTier = this.afterState.properties["accessTier"].ToString();
        }

        var skuIds = StorageAccountSupportedData.CommonSkuToSkuIdMap[sku].ToList();
        if (blobAccessTier == "Cool")
        {
            skuIds.AddRange(StorageAccountSupportedData.CoolTierSkuIds); 
        }
        else
        {
            skuIds.AddRange(StorageAccountSupportedData.HotTierSkuIds);
        }

        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
