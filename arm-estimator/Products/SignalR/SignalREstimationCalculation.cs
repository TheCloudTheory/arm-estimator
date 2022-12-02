using Azure.Core;

internal class SignalREstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public SignalREstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
            double? cost = 0;

            if (item.meterName == "Units")
            {
                cost = item.retailPrice * 30 * capacity;
            }
            else
            {
                cost = item.retailPrice * capacity;
            }

            estimatedCost += cost;
            if (summary.DetailedCost.ContainsKey(item.meterName!))
            {
                summary.DetailedCost[item.meterName!] += cost;
            }
            else
            {
                summary.DetailedCost.Add(item.meterName!, cost);
            }
        }

        summary.TotalCost = estimatedCost.GetValueOrDefault();
        return summary;
    }
}
