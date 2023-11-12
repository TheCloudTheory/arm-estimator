using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class DataFactoryQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH315FBNNW2";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public DataFactoryQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Cloud'";
    }
}
