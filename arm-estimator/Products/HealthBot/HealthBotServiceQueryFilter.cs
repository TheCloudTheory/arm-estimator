using Microsoft.Extensions.Logging;

internal class HealthBotServiceQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH318J9W0D1";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public HealthBotServiceQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Health Bot when SKU is unavailable.");
            return null;
        }

        var skuName = HealthBotServiceSupportedData.SkuToSkuIdMap[sku];
        var skuNameFilter = string.Join(" or ", skuName.Select(_ => $"skuName eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuNameFilter})";
    }
}
