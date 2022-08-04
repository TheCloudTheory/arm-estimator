using Azure.Core;

internal class AKSEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AKSEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changes)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        
        foreach(var item in items)
        {
            // Uptime SLA
            if(item.meterId == "251a08e1-40d6-48e6-8368-4c3e5ee3bbff")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
