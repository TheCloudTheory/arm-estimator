using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class SQLElasticPoolQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3180HX10K";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public SQLElasticPoolQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Azure SQL Elastic Pool when SKU is unavailable.");
            return null;
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

            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ((skuName eq '{sku}' and productName eq 'SQL Database Single/Elastic Pool General Purpose - Compute Gen5') or (productName eq 'SQL Database Single/Elastic Pool General Purpose - Storage' and meterName eq 'General Purpose Data Stored'))";
        }

        var capacity = this.afterState.sku?.capacity;
        if(capacity == null)
        {
            this.logger.LogError("Can't create a filter for Azure SQL Elastic Pool when capacity is unavailable.");
            return null;
        }

        var skuPackName = $"{capacity} DTU Pack";
        if(sku == "Basic")
        {
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and (skuName eq '{skuPackName}' and productName eq 'SQL Database Elastic Pool - Basic')";
        }

        if(IsStandardTierWithAdditionalStorageTier(sku))
        {
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and (skuName eq '{skuPackName}' and productName eq 'SQL Database Elastic Pool - Standard')";
        }

        if(IsPremiumTierWithAdditionalStorageTier(sku))
        {
            return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and (skuName eq '{skuPackName}' and productName eq 'SQL Database Elastic Pool - Premium')";
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuPackName}'";
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
        var tiers = new[] { "Standard" };
        return tiers.Contains(sku);
    }

    private bool IsPremiumTierWithAdditionalStorageTier(string sku)
    {
        var tiers = new[] { "Premium" };
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
