using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class KeyVaultQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3157JCZ2M";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public KeyVaultQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if(sku == null)
        {
            if(this.afterState.properties != null && this.afterState.properties.ContainsKey("sku"))
            {
                var skuData = ((JsonElement)this.afterState.properties["sku"]).Deserialize<Dictionary<string, string>>();
                if(skuData != null && skuData.ContainsKey("name"))
                {
                    sku = skuData["name"];
                }
            }
        }

        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Key Vault when SKU is unavailable.");
            return null;
        }

        var skuName = KeyVaultSupportedData.SkuToSkuNameMap[sku];
        var productName = KeyVaultSupportedData.SkuToProductNameMap[sku];

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}'";
    }
}
