using Microsoft.Extensions.Logging;

internal class SQLQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3180HX10K";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public SQLQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Azure SQL when SKU is unavailable.");
            return null;
        }

        if(sku == "Basic")
        {
            sku = "B";
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}'";
        }

        var skuParts = sku.Split('_');
        if(skuParts.Length > 1)
        {
            // It's vCore model we're talking about here
            sku = $"{skuParts[2]} vCore";
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}' and productName eq 'SQL Database Single/Elastic Pool General Purpose - Compute Gen5'&$top=1";
        }

        if(IsStandardTierWithAdditionalStorageTier(sku))
        {
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and (skuName eq '{sku}' or skuName eq 'Standard')";
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}'";
    }

    private bool IsStandardTierWithAdditionalStorageTier(string sku)
    {
        var tiers = new[] { "S3", "S4", "S6", "S7", "S9", "S12" };
        return tiers.Contains(sku);
    }

    public static int GetNumberOfCoresBasedOnSku(string sku)
    {
        var skuParts = sku.Split('_');
        if(skuParts.Length > 1)
        {
            return int.Parse(skuParts[2]);
        }

        return 0;
    }
}
