using Microsoft.Extensions.Logging;

internal class AnalysisServicesQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH316JSV0PK";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public AnalysisServicesQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Analysis Services when SKU is unavailable.");
            return null;
        }

        if (this.afterState.sku?.capacity != null && this.afterState.sku?.capacity >= 1)
        {
            var scaleOutSku = $"{sku} Scale-Out";
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and (skuName eq '{sku}' or skuName eq '{scaleOutSku}')";
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}'";
    }
}
