using Microsoft.Extensions.Logging;

internal class ConfidentialLedgerQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3182Q9KXV";

    public ConfidentialLedgerQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var skuIds = ConfidentialLedgerSupportedData.SkuToSkuIdMap["Ledger"];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
