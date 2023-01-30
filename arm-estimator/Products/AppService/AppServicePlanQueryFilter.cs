﻿using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class AppServicePlanQueryFilter : IQueryFilter
{
    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public AppServicePlanQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for App Service Plan when SKU is unavailable.");
            return null;
        }

        var skuName = AppServicePlanSupportedData.SkuToSkuNameMap[sku];
        var serviceId = AppServicePlanSupportedData.SkuToServiceId[sku];
        string[] productNames;

        if (IsLinuxPlan())
        {
            productNames = AppServicePlanSupportedData.SkuToProductNameLinuxMap[sku];
        }
        else if (IsLogicApp(sku))
        {
            productNames = new[] { "Logic Apps" }; 
        }
        else
        {
            productNames = AppServicePlanSupportedData.SkuToProductNameWindowsMap[sku];
        }

        var skuIdsFilter = string.Join(" or ", productNames.Select(_ => $"productName eq '{_}'"));

        return $"serviceId eq '{serviceId}' and armRegionName eq '{location}' and skuName eq '{skuName}' and ({skuIdsFilter})";
    }

    private bool IsLinuxPlan()
    {
        var isLinuxPlan = false;
        if (this.afterState.properties != null && this.afterState.properties.ContainsKey("reserved"))
        {
            var isReserved = this.afterState.properties["reserved"].ToString();
            if (isReserved != null)
            {
                isLinuxPlan = bool.Parse(isReserved);
            }
        }

        return isLinuxPlan;
    }

    private bool IsLogicApp(string sku)
    {
        return sku.StartsWith("WS");
    }
}
