﻿using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal abstract class BaseRetailQuery
{
    protected readonly WhatIfChange change;
    protected readonly CommonResourceIdentifier id;
    protected readonly ILogger logger;
    protected readonly string baseQuery;
    protected readonly CurrencyCode currency;
    protected readonly WhatIfChange[] changes;
    protected readonly TemplateSchema template;

    public BaseRetailQuery(WhatIfChange change,
                           CommonResourceIdentifier id,
                           ILogger logger,
                           CurrencyCode currency,
                           WhatIfChange[] changes,
                           TemplateSchema template)
    {
        this.change = change;
        this.id = id;
        this.logger = logger;
        this.baseQuery = $"https://prices.azure.com/api/retail/prices?currencyCode='{currency}'&$filter=priceType eq 'Consumption' and ";
        this.currency = currency;
        this.changes = changes;
        this.template = template;
    }

    public string? GetLocation()
    {
        var location = this.id.GetLocation() ?? (this.id?.GetParent()?.GetLocation());
        return location;
    }
}
