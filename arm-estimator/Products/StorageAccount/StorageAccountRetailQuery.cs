using Azure.Core;
using Microsoft.Extensions.Logging;

internal class StorageAccountRetailQuery
{
    private readonly WhatIfChange change;
    private readonly ILogger logger;

    public StorageAccountRetailQuery(WhatIfChange change, ILogger logger)
    {
        this.change = change;
        this.logger = logger;
    }

    public string? GetQueryUrl()
    {
        if(this.change.after == null)
        {
            this.logger.LogError("Can't generate Retail API query if desired state is unavailable.");
            return null;
        }

        var filter = new StorageAccountQueryFilter(this.change.after).GetFiltersBasedOnDesiredState();
        return $"https://prices.azure.com/api/retail/prices?{filter}";
    }
}
