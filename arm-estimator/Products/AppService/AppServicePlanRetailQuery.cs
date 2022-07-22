using Microsoft.Extensions.Logging;

internal class AppServicePlanRetailQuery : BaseRetailQuery, IRetailQuery
{
    public AppServicePlanRetailQuery(WhatIfChange change, ILogger logger)
        : base(change, logger)
    {
    }

    public string? GetQueryUrl()
    {
        if (this.change.after == null)
        {
            this.logger.LogError("Can't generate Retail API query if desired state is unavailable.");
            return null;
        }

        var filter = new AppServicePlanQueryFilter(this.change.after, this.logger).GetFiltersBasedOnDesiredState();
        return $"https://prices.azure.com/api/retail/prices?{filter}";
    }
}
