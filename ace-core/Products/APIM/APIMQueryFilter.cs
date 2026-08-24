using ACE.WhatIf;
using Microsoft.Extensions.Logging;

namespace ACE.Products.APIM;

internal class ApimQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger) : IQueryFilter
{
    private const string ServiceId = "DZH318FZ20SD";

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = afterState.sku?.name;
        var type = afterState.type;

        if(type is "Microsoft.ApiManagement/service/gateways")
        {
            sku = "Gateway";
        }

        switch (sku)
        {
            case null when (afterState.properties?.ContainsKey("sku_name") == false):
                logger.LogError("Can't create a filter for API Management Service when SKU is unavailable.");
                return null;
            case null when (afterState.properties?.ContainsKey("sku_name") == true):
            {
                var skuName = afterState.properties!["sku_name"];
                sku = skuName!.ToString()!.Split('_')[0];
                break;
            }
        }

        if (sku == null)
        {
            throw new InvalidFilterDataException("Can't create a filter for API Management Service when SKU is unavailable.");
        }

        var skuNames = APIMSupportedData.SkuToSkuNameMap[sku];
        var skuNamesFilter = string.Join(" or ", skuNames.Select(s => $"skuName eq '{s}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuNamesFilter})";
    }
}