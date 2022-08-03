﻿using Azure.Core;
using Microsoft.Extensions.Logging;

internal class StreamAnalyticsRetailQuery : BaseRetailQuery, IRetailQuery
{
    public StreamAnalyticsRetailQuery(WhatIfChange change, ResourceIdentifier id, ILogger logger)
        : base(change, id, logger)
    {
    }

    public string? GetQueryUrl(string location)
    {
        if (this.change.after == null && this.change.before == null)
        {
            this.logger.LogError("Can't generate Retail API query if desired state is unavailable.");
            return null;
        }

        var change = this.change.after ?? this.change.before;
        if(change == null)
        {
            this.logger.LogError("Couldn't determine after / before state.");
            return null;
        }

        var filter = new StreamAnalyticsQueryFilter(change, this.logger).GetFiltersBasedOnDesiredState(location);
        return $"https://prices.azure.com/api/retail/prices?{filter}";
    }
}