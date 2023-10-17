using ACE.WhatIf;
using Azure.Core;
using Microsoft.Extensions.Logging;

internal class MariaDBRetailQuery : BaseRetailQuery, IRetailQuery
{
    public MariaDBRetailQuery(WhatIfChange change, CommonResourceIdentifier id, ILogger logger, CurrencyCode currency, WhatIfChange[] changes, TemplateSchema template) : base(change, id, logger, currency, changes, template)
    {
    }

    public RetailAPIResponse? GetFakeResponse()
    {
        throw new NotImplementedException();
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

        var filter = new MariaDBQueryFilter(change, this.logger).GetFiltersBasedOnDesiredState(location);
        return $"{baseQuery}{filter}";
    }
}
