using Azure.Core;

internal class TimeSeriesEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public TimeSeriesEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var capacity = this.change.sku?.capacity;
        var summary = new TotalCostSummary();

        if (capacity == null)
        {
            capacity = 1;
        }

        foreach (var item in items)
        {
            var cost = item.retailPrice * 30 * capacity;

            estimatedCost += cost;
            summary.DetailedCost.Add(item.meterName!, cost);
        }

        return summary;
    }
}
