using Microsoft.Extensions.Logging;

internal class RecoveryServicesQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3155G7B5D";

    public RecoveryServicesQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Standard' and meterName eq 'GRS Data Stored'";
    }
}
