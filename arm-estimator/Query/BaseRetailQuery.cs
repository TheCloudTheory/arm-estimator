using Microsoft.Extensions.Logging;

internal abstract class BaseRetailQuery
{
    protected readonly WhatIfChange change;
    protected readonly ILogger logger;

    public BaseRetailQuery(WhatIfChange change, ILogger logger)
    {
        this.change = change;
        this.logger = logger;
    }
}
