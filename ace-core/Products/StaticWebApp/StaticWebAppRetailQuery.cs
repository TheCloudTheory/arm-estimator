using ACE.Extensions;
using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class StaticWebAppRetailQuery(WhatIfChange change, CommonResourceIdentifier id, ILogger logger, CurrencyCode currency, WhatIfChange[] changes, TemplateSchema template) 
    : BaseRetailQuery(change, id, logger, currency, changes, template), IRetailQuery
{
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

        var change = this.change.GetChange();
        if (change == null)
        {
            this.logger.LogError("Couldn't determine after / before state.");
            return null;
        }

        var filter = new StaticWebAppQueryFilter(change, this.logger).GetFiltersBasedOnDesiredState(location);
        if(filter == "FREE")
        {
            return filter;
        }

        return $"{baseQuery}{filter}";
    }
}
