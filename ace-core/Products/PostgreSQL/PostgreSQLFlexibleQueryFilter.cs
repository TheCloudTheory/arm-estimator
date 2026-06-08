using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class PostgreSQLFlexibleQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger) : IQueryFilter
{
    private const string ServiceId = "DZH3199QPQTD";

    private readonly WhatIfAfterBeforeChange afterState = afterState;
    private readonly ILogger logger = logger;

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

        string? skuProductName;
        string? storageProductName;
        string? productName;
        string? skuName;

        // Note that for some reasons some product names for PostgreSQL contain non-breaking space \u00A0
        // meaning query won't work if we miss them. We may use armSkuName in the future
        if (tierId == "Burstable")
        {
            // Always fun when there're magical edge cases :F
            if(familyId.Equals("B1ms", StringComparison.CurrentCultureIgnoreCase) || familyId.Equals("B2s", StringComparison.CurrentCultureIgnoreCase))
            {
                familyId = familyId.ToUpper();
            }

            skuName = familyId;
            storageProductName = "Az DB for PostgreSQL Flexible Server Storage";
            productName = $"Azure Database for PostgreSQL Flexible Server Burstable BS Series Compute";
        }
        else if (tierId == "GeneralPurpose")
        {
            skuName = $"{skuParts[2]} vCore";
            skuProductName = $"General Purpose\u00A0{familyId}\u00A0";
            storageProductName = "Az DB for PostgreSQL Flexible Server Storage";
            productName = $"Azure Database for PostgreSQL Flexible Server {skuProductName}Series Compute";
        }
        else
        {
            skuName = $"{skuParts[2]} vCore";
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
