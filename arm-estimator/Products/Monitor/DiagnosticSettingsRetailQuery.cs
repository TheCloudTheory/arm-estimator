using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class DiagnosticSettingsRetailQuery : BaseRetailQuery, IRetailQuery
{
    public DiagnosticSettingsRetailQuery(WhatIfChange change, CommonResourceIdentifier id, ILogger logger, CurrencyCode currency, WhatIfChange[] changes) : base(change, id, logger, currency, changes)
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

        var change = this.change.after == null ? this.change.before : this.change.after;
        if (change == null)
        {
            this.logger.LogError("Couldn't determine after / before state.");
            return null;
        }

        var filters = new List<string>();
        if (change.properties != null)
        {
            if(change.properties.ContainsKey("workspaceId"))
            {
                var laFilter = new LogAnalyticsQueryFilter(new WhatIfAfterBeforeChange()
                {
                    type = "Microsoft.OperationalInsights/workspaces"
                }, this.logger).GetFiltersBasedOnDesiredState(location);

                if(laFilter == null)
                {
                    this.logger.LogError("Failed to obtain Log Analytics filter for Diagnostic Settings.");
                    return null;
                }

                filters.Add($"({laFilter})");
            }

            if(change.properties.ContainsKey("storageAccountId"))
            {
                var storageFilter = new StorageAccountQueryFilter(new WhatIfAfterBeforeChange()
                {
                    sku = new WhatIfSku()
                    {
                        name = "Standard_LRS"
                    }
                }, this.logger).GetFiltersBasedOnDesiredState(location);

                if (storageFilter == null)
                {
                    this.logger.LogError("Failed to obtain Storage Account filter for Diagnostic Settings.");
                    return null;
                }

                filters.Add($"({storageFilter})");
            }

            if (change.properties.ContainsKey("eventHubName"))
            {
                var ehFilter = new EventHubQueryFilter(new WhatIfAfterBeforeChange()
                {
                    type = "Microsoft.EventHub/namespaces",
                    sku = new WhatIfSku()
                    {
                        name = "Basic"
                    }
                }, this.logger).GetFiltersBasedOnDesiredState(location);

                if (ehFilter == null)
                {
                    this.logger.LogError("Failed to obtain Event Hub filter for Diagnostic Settings.");
                    return null;
                }

                filters.Add($"({ehFilter})");
            }
        }

        var filter = string.Join(" or ", filters);
        return $"{baseQuery}{filter}";
    }
}
