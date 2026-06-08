using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class CognitiveSearchQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH318X5DGJB";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public CognitiveSearchQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Cognitive Search when SKU is unavailable.");
            return null;
        }

        var skuName = CognitiveSearchSupportedData.SkuToSkuNameMap[sku];
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}'";
    }
}
