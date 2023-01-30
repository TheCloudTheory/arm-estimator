using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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

        string? skuProductName;
        string? storageSkuName;

        if (tierId == "B")
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

        if (this.afterState.properties != null && this.afterState.properties.ContainsKey("storageProfile"))
        {
            var storageProfile = ((JsonElement)this.afterState.properties["storageProfile"]).Deserialize<MariaDBStorageProfile>();
            if (storageProfile != null)
            {
                var backupSku = "Backup LRS";
                if (storageProfile.geoRedundantBackup != null && storageProfile.geoRedundantBackup == "Enabled")
                {
                    backupSku = "Backup GRS";
                }

                return $"serviceId eq '{ServiceId}' and ((armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}') or (armRegionName eq '{location}' and skuName eq '{storageSkuName}') or (armRegionName eq '{location}' and skuName eq '{backupSku}'))";
            }
        }

        return $"serviceId eq '{ServiceId}' and ((armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}') or (armRegionName eq '{location}' and skuName eq '{storageSkuName}'))";
    }
}
