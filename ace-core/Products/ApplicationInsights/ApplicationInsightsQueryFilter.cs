using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class ApplicationInsightsQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH319K8RFPB";
    private const string LogAnalyticsServiceId = "DZH3140GXVMF";

    public ApplicationInsightsQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var skuIds = ApplicationInsightsSupportedData.SkuToSkuNameMap["classic"];
        var skuNamesFilter = string.Join(" or ", skuIds.Select(_ => $"skuName eq '{_}'"));

        return $"armRegionName eq '{location}' and ((serviceId eq '{ServiceId}' and ({skuNamesFilter})) or (serviceId eq '{LogAnalyticsServiceId}' and ({skuNamesFilter})))";
    }
}
