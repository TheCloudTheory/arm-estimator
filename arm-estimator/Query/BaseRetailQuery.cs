using Azure.Core;
using Microsoft.Extensions.Logging;

internal abstract class BaseRetailQuery
{
    protected readonly WhatIfChange change;
    protected readonly ResourceIdentifier id;
    protected readonly ILogger logger;

    protected static string BaseQuery => "https://prices.azure.com/api/retail/prices?$filter=priceType eq 'Consumption' and ";

    public BaseRetailQuery(WhatIfChange change, ResourceIdentifier id, ILogger logger)
    {
        this.change = change;
        this.id = id;
        this.logger = logger;
    }

    public string? GetLocation()
    {
        var location = this.id.Location == null ? this.id?.Parent?.Location : this.id.Location;
        return location;
    }
}
