using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class SignalRQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH319BFCV72";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public SignalRQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for SignalR when SKU is unavailable.");
            return null;
        }

        if(sku == "Free_F1")
        {
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and meterName eq 'Units - Free'";
        }

        var skuName = SignalRSupportedData.SkuToSkuNameMap[sku];
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}'";
    }
}
