using Azure.Core;

internal class ChaosEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public ChaosEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();

        foreach (var item in items)
        {
            // Assumption - if one uses Chaos Studio, let's say they're leveraging
            // its capabilities each day for 15 minutes
            estimatedCost += item.retailPrice * 15 * 30;
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
