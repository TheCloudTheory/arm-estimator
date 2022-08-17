using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class PublicIPQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH314HC0WV9";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public PublicIPQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
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

        if(this.afterState.properties == null)
        {
            this.logger.LogError("Can't create a filter for Public IP Address when properties are unavailable.");
            return null;
        }

        if(tier == "Global")
        {
            sku = "Global";
        }

        var properties = JsonSerializer.Deserialize<PublicIPProperties>(JsonSerializer.Serialize(this.afterState.properties));
        string? metricName;
        if(sku == "Basic")
        {
            if(properties?.publicIPAllocationMethod == "Static")
            {
                metricName = "Static Public IP";
            }
            else
            {
                metricName = "Dynamic Public IP";
            }
        }
        else if (sku == "Standard")
        {
            if(properties?.publicIpAddressVersion == "IPv6")
            {
                metricName = "Standard Static IP Addresses";
            }
            else
            {
                metricName = "Standard IPv4 Static Public IP";
            }
        }
        else
        {
            if (properties?.publicIpAddressVersion == "IPv6")
            {
                metricName = "Global Static Public IP";
            }
            else
            {
                metricName = "Global IPv4";
            }
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}' and meterName eq '{metricName}'";
    }
}
