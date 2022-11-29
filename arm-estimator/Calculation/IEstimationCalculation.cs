internal interface IEstimationCalculation
{
    double GetTotalCost(WhatIfChange[] changess, IDictionary<string, string>? usagePatterns);
    IOrderedEnumerable<RetailItem> GetItems();
}
