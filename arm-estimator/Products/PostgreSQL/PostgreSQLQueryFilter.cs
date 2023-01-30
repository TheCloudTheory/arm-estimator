using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class PostgreSQLQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3199QPQTD";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public PostgreSQLQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
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

        var skuParts = sku.Split("_");
        var tierId = skuParts[0];
        var familyId = skuParts[1];
        var cores = skuParts[2];
        string? skuProductName;
        string? storageProductName;

        if (tierId == "B")
        {
            skuProductName = "Basic";
            storageProductName = "Azure Database for PostgreSQL Single Server Basic - Storage";
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

        var skuName = $"{cores} vCore";
        var productName = $"Azure Database for PostgreSQL Single Server {skuProductName} - Compute {familyId}";

        if (this.afterState.properties != null && this.afterState.properties.ContainsKey("storageProfile"))
        {
            var storageProfile = ((JsonElement)this.afterState.properties["storageProfile"]).Deserialize<PostgreSQLStorageProfile>();
            if (storageProfile != null)
            {
                var backupSku = "Backup LRS";
                if (storageProfile.geoRedundantBackup != null && storageProfile.geoRedundantBackup == "Enabled")
                {
                    backupSku = "Backup GRS";
                }

                return $"serviceId eq '{ServiceId}' and ((armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}') or (armRegionName eq '{location}' and productName eq '{storageProductName}') or (armRegionName eq '{location}' and skuName eq '{backupSku}' and productName eq 'Azure Database for PostgreSQL Single Server - Backup Storage'))";
            }
        }

        return $"serviceId eq '{ServiceId}' and ((armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}') or (armRegionName eq '{location}' and productName eq '{storageProductName}'))";
    }
}
