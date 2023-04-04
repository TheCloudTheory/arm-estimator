using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class FirewallQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH318G79HZT";

    private readonly WhatIfAfterBeforeChange afterState;

    public FirewallQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var skuName = "Standard";
        if (this.afterState.properties != null && this.afterState.properties.ContainsKey("sku"))
        {
            var sku = ((JsonElement)this.afterState.properties["sku"]!).Deserialize<FirewallSkuData>();
            if(sku?.tier != null)
            {
                skuName = sku.tier;
            }
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}'";
    }
}
