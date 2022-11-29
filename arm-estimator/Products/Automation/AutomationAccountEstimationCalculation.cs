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

    public double GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();

        foreach (var item in items)
        {
            if (item.meterName == "Watcher")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Basic Runtime")
            {
                estimatedCost += item.retailPrice * 500; // Give it the same amount as free tier
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
