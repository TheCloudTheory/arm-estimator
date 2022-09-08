using Microsoft.Extensions.Logging;

internal class MonitorQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH315BMCLC8";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public MonitorQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        if(this.afterState.location == "global")
        {
            // TODO: #16 - to be changed when implemented
            return $"serviceId eq '{ServiceId}' and armRegionName eq 'eastus2' and skuName eq 'Alerts'";
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Alerts'";
    }
}
