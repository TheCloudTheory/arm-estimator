using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class AppConfigurationQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH319K8RFPB";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public AppConfigurationQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for App Configuration when SKU is unavailable.");
            return null;
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{MakeSkuUppercase(sku)}'";
    }

    /// <summary>
    /// This method is intended to fix a special case when SKU is defined in Terraform.
    /// There's difference between ARM/Bicep and TF as the former provides correct value with
    /// capital first letter. As queries against Retail API are case-sensitive, we cannot
    /// pass SKU from TF, which is (as of today) written in lower-case only.
    /// </summary>
    private static string? MakeSkuUppercase(string? sku)
    {
        if (sku == null) return null;

        return sku[0].ToString().ToUpper() + sku[1..];
    }
}
