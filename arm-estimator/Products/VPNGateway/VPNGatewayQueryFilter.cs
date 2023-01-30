using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class VPNGatewayQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH314W50487";
    private const string ExpressRouteServiceId = "DZH319F2869N";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public VPNGatewayQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        string? sku = null;
        if (this.afterState.properties != null && this.afterState.properties.ContainsKey("sku"))
        {
            var skuData = ((JsonElement)this.afterState.properties["sku"]).Deserialize<Dictionary<string, string>>();
            if (skuData != null && skuData.ContainsKey("name"))
            {
                sku = skuData["name"];
            }
        }

        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for VPN Gateway when SKU is unavailable.");
            return null;
        }

        var serviceId = ServiceId;
        if(sku == "Standard" || sku == "ErGw1AZ" || sku == "ErGw2AZ" || sku == "ErGw3AZ" || sku == "HighPerformance" || sku == "UltraPerformance")
        {
            serviceId = ExpressRouteServiceId;
        }

        return $"serviceId eq '{serviceId}' and armRegionName eq '{location}' and skuName eq '{sku}'";
    }
}
