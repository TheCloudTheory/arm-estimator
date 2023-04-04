using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System;
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
        string? productName;

        // Note that for some reasons some product names for PostgreSQL contain non-breaking space \u00A0
        // meaning query won't work if we miss them. We may use armSkuName in the future
        if (tierId == "Burstable")
        {
            skuName = familyId;
            skuProductName = "Burstable BS";
            storageProductName = "Az DB for PostgreSQL Flexible Server Storage";
            productName = $"Azure Database for PostgreSQL Flexible Server {skuProductName} Series Compute";
        }
        else if (tierId == "GeneralPurpose")
        {
            skuProductName = $"General Purpose\u00A0{familyId}\u00A0";
            storageProductName = "Az DB for PostgreSQL Flexible Server Storage";
            productName = $"Azure Database for PostgreSQL Flexible Server {skuProductName}Series Compute";
        }
        else
        {
            skuProductName = $"Memory Optimized\u00A0{familyId}\u00A0";
            storageProductName = "Az DB for PostgreSQL Flexible Server Storage";

            if (familyId == "Edsv4")
            {
                productName = $"Azure\u00A0Database for PostgreSQL Flexible Server {skuProductName}Series Compute";
            }
            else
            {
                productName = $"Azure\u00A0Database for PostgreSQL Flexible Server\u00A0{skuProductName}Series Compute";
            }
        }

        if (this.afterState.properties != null && this.afterState.properties.ContainsKey("storageProfile"))
        {
            var storageProfile = ((JsonElement)this.afterState.properties["storageProfile"]!).Deserialize<PostgreSQLStorageProfile>();
            if (storageProfile != null)
            {
                return $"serviceId eq '{ServiceId}' and ((armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}') or (armRegionName eq '{location}' and productName eq '{storageProductName}') or (armRegionName eq '{location}' and skuName eq 'Backup Storage LRS'))";
            }
        }

        return $"serviceId eq '{ServiceId}' and ((armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}') or (armRegionName eq '{location}' and productName eq '{storageProductName}'))";
    }
}
