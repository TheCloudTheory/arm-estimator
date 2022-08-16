using Microsoft.Extensions.Logging;

internal class ChaosQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3171F2TXD";

    public ChaosQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Basic'";
    }
}
