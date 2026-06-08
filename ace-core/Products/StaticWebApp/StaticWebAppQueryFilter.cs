using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class StaticWebAppQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger) : IQueryFilter
{
    private const string ServiceId = "DZH317GT8G5N";

    private readonly WhatIfAfterBeforeChange afterState = afterState;
    private readonly ILogger logger = logger;

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Static Web App when SKU is unavailable.");
            return null;
        }

        if(sku == "Free")
        {
            return "FREE";
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}' and productName eq 'Static Web Apps' and meterName eq 'Standard App'";
    }
}
