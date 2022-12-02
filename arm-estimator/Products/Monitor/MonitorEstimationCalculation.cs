using Azure.Core;

internal class MonitorEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public MonitorEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public TotalCostSummary GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        var summary = new TotalCostSummary();

        foreach (var item in items)
        {
            var cost = item.retailPrice;

            estimatedCost += cost;
            summary.DetailedCost.Add(item.meterName!, cost);
        }

        return summary;
    }
}
