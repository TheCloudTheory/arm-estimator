using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class LogAnalyticsQueryFilter : IQueryFilter
{
    internal const string ServiceId = "DZH3140GXVMF";
    private const string SentinelServiceId = "DZH317M2MHR4";
    private const string MonitorServiceId = "DZH315BMCLC8";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public LogAnalyticsQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        if (this.afterState.type == null)
        {
            this.logger.LogError("Can't create a filter for Log Analytics when type is unavailable.");
            return null;
        }

        if (this.afterState.type == "Microsoft.OperationalInsights/workspaces")
        {
            return $"(serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Pay-as-you-go') or (serviceId eq '{MonitorServiceId}' and armRegionName eq '{location}' and skuName eq 'Basic Logs')";
        }
        else
        {
            if (this.afterState.plan == null)
            {
                this.logger.LogError("Can't create a filter for Log Analytics when plan is unavailable.");
                return null;
            }

            return $"serviceId eq '{SentinelServiceId}' and armRegionName eq '{location}' and (skuName eq 'Basic Logs' or skuName eq 'Pay-as-you-go')";
        }
    }
}
