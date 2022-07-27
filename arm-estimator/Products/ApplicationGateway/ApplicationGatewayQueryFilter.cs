using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class ApplicationGatewayQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH319QDM5ZN";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public ApplicationGatewayQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        if(this.afterState.properties == null)
        {
            this.logger.LogError("Can't create a filter for Application Gateway when properties are not unavailable.");
            return null;
        }

        if (this.afterState.properties.TryGetValue("sku", out var skuData) == false)
        {
            this.logger.LogError("Can't create a filter for Application Gateway when SKU is unavailable.");
            return null;
        }

        if (skuData == null)
        {
            this.logger.LogError("Can't create a filter for Application Gateway when SKU is unavailable.");
            return null;
        }

        var sku = ((JsonElement)skuData).Deserialize<Dictionary<string, object>>();
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Application Gateway when SKU is invalid.");
            return null;
        }

        if(sku.ContainsKey("name") == false)
        {
            this.logger.LogError("Can't create a filter for Application Gateway when SKU is invalid.");
            return null;
        }

        var skuName = sku["name"]?.ToString();
        if(skuName == null)
        {
            this.logger.LogError("Can't create a filter for Application Gateway when SKU is invalid.");
            return null;
        }

        var skuIds = ApplicationGatewaySupportedData.SkuToSkuIdMap[skuName];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
