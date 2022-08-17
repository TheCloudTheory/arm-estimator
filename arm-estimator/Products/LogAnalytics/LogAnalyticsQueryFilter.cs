using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class LogAnalyticsQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3140GXVMF";
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
        if(this.afterState.type == null)
        {
            this.logger.LogError("Can't create a filter for Log Analytics when type is unavailable.");
            return null;
        }

        if (this.afterState.type == "Microsoft.OperationalInsights/workspaces")
        {
            if(this.afterState.properties != null && this.afterState.properties.ContainsKey("sku"))
            {
                var skuData = ((JsonElement)this.afterState.properties["sku"]).Deserialize<LogAnalyticsSku>();
                if(skuData == null || skuData.name == null)
                {
                    this.logger.LogError("Can't create a filter for Log Analytics when SKU is unavailable.");
                    return null;
                }

                return $"(serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Pay-as-you-go') or (serviceId eq '{MonitorServiceId}' and armRegionName eq '{location}' and skuName eq 'Basic Logs')";
            }
            else
            {
                this.logger.LogError("Can't create a filter for Log Analytics when SKU is unavailable.");
                return null;
            }
        }
        else
        {
            if(this.afterState.plan == null)
            {
                this.logger.LogError("Can't create a filter for Log Analytics when plan is unavailable.");
                return null;
            }

            return $"serviceId eq '{SentinelServiceId}' and armRegionName eq '{location}' and (skuName eq 'Basic Logs' or skuName eq 'Pay-as-you-go')";
        }
    }
}
