using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class APIMQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH318FZ20SD";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public APIMQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        var type = this.afterState.type;

        if(type != null && type == "Microsoft.ApiManagement/service/gateways")
        {
            sku = "Gateway";
        }

        if (sku == null && this.afterState.properties?.ContainsKey("sku_name") == false)
        {
            this.logger.LogError("Can't create a filter for API Management Service when SKU is unavailable.");
            return null;
        }
        else
        {
            var skuName = this.afterState.properties!["sku_name"];
            sku = skuName!.ToString()!.Split('_')[0];
        }

        var skuNames = APIMSupportedData.SkuToSkuNameMap[sku!];
        var skuNamesFilter = string.Join(" or ", skuNames.Select(_ => $"skuName eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuNamesFilter})";
    }
}
