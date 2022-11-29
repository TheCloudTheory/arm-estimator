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

    public double GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        var capacity = this.change.sku?.capacity;
        
        if(capacity == null)
        {
            capacity = 1;
        }

        foreach (var item in items)
        {
            if (item.meterName == "Units")
            {
                estimatedCost += item.retailPrice * 30 * capacity;
            }
            else
            {
                estimatedCost += item.retailPrice * capacity;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
