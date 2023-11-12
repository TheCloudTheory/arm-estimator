    using ACE.WhatIf;
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

            
            if(IsZoneRedundantDatabase())
            {
                sku += " Zone Redundancy";
            }

            var tier = "General Purpose";
            if(skuParts[0] == "BC")
            {
                tier = "Business Critical";

                return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ((skuName eq '{sku}' and productName eq 'SQL Database Single/Elastic Pool {tier} - Compute Gen5') or (productName eq 'SQL Database Single/Elastic Pool Business Critical - Storage'))";
            }

            if(skuParts[0] == "HS")
            {
                return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ((skuName eq '{sku}' and productName eq 'SQL Database SingleDB/Elastic Pool Hyperscale - Compute Gen5') or (skuName eq 'Hyperscale' and productName eq 'SQL Database SingleDB Hyperscale - Storage'))";
            }


            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ((skuName eq '{sku}' and productName eq 'SQL Database Single/Elastic Pool {tier} - Compute Gen5') or (productName eq 'SQL Database Single/Elastic Pool General Purpose - Storage' and meterName eq 'General Purpose Data Stored'))";
        }

        if(IsStandardTierWithAdditionalStorageTier(sku))
        {
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and (skuName eq '{sku}' or skuName eq 'Standard')";
        }

        if(IsPremiumTierWithAdditionalStorageTier(sku))
        {
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and (skuName eq '{sku}' or skuName eq 'Premium')";
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}'";
    }

    private bool IsZoneRedundantDatabase()
    {
        if (this.afterState.properties == null) return false;
        if (this.afterState.properties.ContainsKey("zoneRedundant") == false) return false;

        var isZrs = bool.Parse(this.afterState.properties["zoneRedundant"]!.ToString()!);
        return isZrs;
    }

    private bool IsStandardTierWithAdditionalStorageTier(string sku)
    {
        var tiers = new[] { "S3", "S4", "S6", "S7", "S9", "S12" };
        return tiers.Contains(sku);
    }

    private bool IsPremiumTierWithAdditionalStorageTier(string sku)
    {
        var tiers = new[] { "P1", "P2", "P4", "P6" };
        return tiers.Contains(sku);
    }

    // Calculate number of cores based on SKU name. As Azure SQL SKUs
    // for vCore model are in format like "GP_Gen5_2", we can extract
    // number of cores from the SKU name and use it in calculations.
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
