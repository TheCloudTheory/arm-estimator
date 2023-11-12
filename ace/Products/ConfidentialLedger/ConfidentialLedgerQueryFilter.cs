using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class ConfidentialLedgerQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3182Q9KXV";

    public ConfidentialLedgerQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var skuNames = ConfidentialLedgerSupportedData.SkuToSkuNameMap["Ledger"];
        var skuNamesFilter = string.Join(" or ", skuNames.Select(_ => $"skuName eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuNamesFilter})";
    }
}
