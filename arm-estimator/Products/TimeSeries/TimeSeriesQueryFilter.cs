using Microsoft.Extensions.Logging;

internal class TimeSeriesQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH317Z6VZ3V";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public TimeSeriesQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Time Series when SKU is unavailable.");
            return null;
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}'";
    }
}
