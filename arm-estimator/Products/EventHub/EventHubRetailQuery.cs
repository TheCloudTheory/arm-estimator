﻿using Azure.Core;
using Microsoft.Extensions.Logging;

internal class EventHubRetailQuery : BaseRetailQuery, IRetailQuery
{
    public EventHubRetailQuery(WhatIfChange change, ResourceIdentifier id, ILogger logger)
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

        var filter = new EventHubQueryFilter(change, this.logger).GetFiltersBasedOnDesiredState(location);
        return $"https://prices.azure.com/api/retail/prices?{filter}";
    }
}