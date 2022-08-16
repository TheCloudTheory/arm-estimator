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

    public string? GetFiltersBasedOnDesiredState(string location)
    { 
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

        var skuName = StorageAccountSupportedData.CommonSkuToSkuIdMap[sku];
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}'";
    }
}
