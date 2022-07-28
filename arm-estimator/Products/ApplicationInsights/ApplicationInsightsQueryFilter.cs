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
        var skuIds = ApplicationInsightsSupportedData.SkuToSkuIdMap["classic"];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=(serviceId eq '{ServiceId}' or serviceId eq '{LogAnalyticsServiceId}') and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
