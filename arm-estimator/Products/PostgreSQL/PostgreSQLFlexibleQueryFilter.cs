using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class PostgreSQLFlexibleQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3199QPQTD";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public PostgreSQLFlexibleQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for PostgreSQL when SKU is unavailable.");
            return null;
        }

        var tierId = this.afterState.sku?.tier;
        var skuParts = sku.Split("_");
        var familyId = skuParts[1];
        var cores = skuParts[2];
        var skuName = $"{cores} vCore";
        string? skuProductName;
        string? storageProductName;

        if (tierId == "Burstable")
        {
            skuName = familyId;
            skuProductName = "Burstable BS";
            storageProductName = "Az DB for PostgreSQL Flexible Server Storage";
        }
        else if (tierId == "GP")
        {
            skuProductName = "General Purpose";
            storageProductName = "Azure Database for PostgreSQL Single Server General Purpose - Storage";
        }
        else
        {
            skuProductName = "Memory Optimized";
            storageProductName = "Azure Database for PostgreSQL Single Server General Purpose - Storage";
        }

        var productName = $"Azure Database for PostgreSQL Flexible Server {skuProductName} Series Compute";

        if (this.afterState.properties != null && this.afterState.properties.ContainsKey("storageProfile"))
        {
            var storageProfile = ((JsonElement)this.afterState.properties["storageProfile"]).Deserialize<PostgreSQLStorageProfile>();
            if(storageProfile != null)
            {
                if(this.afterState.sku?.size != null && storageProfile.storageMB > int.Parse(this.afterState.sku?.size!))
                {
                    var backupSku = "Backup LRS";
                    if (storageProfile.geoRedundantBackup != null && storageProfile.geoRedundantBackup == "Enabled")
                    {
                        backupSku = "Backup GRS";
                    }

                    return $"serviceId eq '{ServiceId}' and ((armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}') or (armRegionName eq '{location}' and skuName eq '{storageProductName}') or (armRegionName eq '{location}' and skuName eq '{backupSku}'))";
                }
            }
        }

        return $"serviceId eq '{ServiceId}' and ((armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}') or (armRegionName eq '{location}' and productName eq '{storageProductName}'))";
    }
}
