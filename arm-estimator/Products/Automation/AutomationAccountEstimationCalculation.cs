using Azure.Core;

internal class AutomationAccountEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AutomationAccountEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
            double? cost = 0;

            if (item.meterName == "Watcher")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Basic Runtime")
            {
                cost = item.retailPrice * 500; // Give it the same amount as free tier
            }
            else
            {
                cost = item.retailPrice;
            }

            estimatedCost += cost;
            summary.DetailedCost.Add(item.meterName!, cost);
        }

        return summary;
    }
}
