using Microsoft.Extensions.Logging;

internal class PublicIPPrefixQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH314HC0WV9";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public PublicIPPrefixQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        var tier = this.afterState.sku?.tier;
        if (sku == null || tier == null)
        {
            this.logger.LogError("Can't create a filter for Public IP Address when SKU is unavailable.");
            return null;
        }

        if (this.afterState.properties == null)
        {
            this.logger.LogError("Can't create a filter for Public IP Address when properties are unavailable.");
            return null;
        }

        if (tier == "Global")
        {
            sku = "Global";
        }

        string? metricName;
        if (sku == "Standard")
        {
            metricName = "Standard Static IP Addresses";
        }
        else
        {
            metricName = "Global Static Public IP";
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}' and meterName eq '{metricName}'";
    }
}
