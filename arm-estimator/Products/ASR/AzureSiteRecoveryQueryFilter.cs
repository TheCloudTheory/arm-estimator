using Microsoft.Extensions.Logging;

internal class AzureSiteRecoveryQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3174MS2N2";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public AzureSiteRecoveryQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Azure'";
    }
}
