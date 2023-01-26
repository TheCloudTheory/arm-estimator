using Microsoft.Extensions.Logging;

internal class StorageAccountQueryFilter : IQueryFilter
{
    internal const string ServiceId = "DZH317F1HKN0";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public StorageAccountQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    { 
        var sku = this.afterState.sku?.name;

        if(sku == null)
        {
            this.logger.LogError("Can't create a filter for Storage Account when SKU is unavailable.");
            return null;
        }

        var skuName = StorageAccountSupportedData.CommonSkuToSkuIdMap[sku];
        if(IsPremium(sku))
        {
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq 'Premium Block Blob'";
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}' and (productName eq 'Tables' or productName eq 'Queues v2' or productName eq 'Tables' or productName eq 'General Block Blob')";
    }

    private bool IsPremium(string sku)
    {
        return sku.StartsWith("Premium");
    }
}
