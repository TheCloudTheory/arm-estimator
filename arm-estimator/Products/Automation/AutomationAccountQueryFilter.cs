using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class AutomationAccountQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH315GS6SRD";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public AutomationAccountQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Basic' and productName eq 'Process Automation'";
    }
}
