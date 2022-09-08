using Microsoft.Extensions.Logging;

internal class MariaDBQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH31974T4H6";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public MariaDBQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Maria DB when SKU is unavailable.");
            return null;
        }

        var skuParts = sku.Split("_");
        var tierId = skuParts[0];
        var familyId = skuParts[1];
        var cores = skuParts[2];
        string? skuProductName = null;
        string? storageSkuName = null;

        if(tierId == "B")
        {
            skuProductName = "Basic";
            storageSkuName = "Basic";
        }
        else if (tierId == "GP")
        {
            skuProductName = "General Purpose";
            storageSkuName = "General Purpose";
        }
        else
        {
            skuProductName = "Memory Optimized";
            storageSkuName = "Perf Optimized";
        }

        var skuName = $"{cores} vCore";
        var productName = $"Azure Database for MariaDB Single Server {skuProductName} - Compute {familyId}";

        return $"serviceId eq '{ServiceId}' and ((armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}') or (armRegionName eq '{location}' and skuName eq '{storageSkuName}'))";
    }
}
