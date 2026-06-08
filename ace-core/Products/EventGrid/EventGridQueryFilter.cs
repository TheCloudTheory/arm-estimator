using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class EventGridQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH315NX4WLD";

    public EventGridQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Standard'";
    }
}
