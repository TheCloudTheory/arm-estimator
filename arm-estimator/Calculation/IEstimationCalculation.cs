namespace ACE.Calculation;

internal interface IEstimationCalculation
{
    TotalCostSummary GetTotalCost(WhatIfChange[] changess, IDictionary<string, string>? usagePatterns);
    IOrderedEnumerable<RetailItem> GetItems();
}