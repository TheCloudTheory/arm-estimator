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

    public double GetTotalCost(WhatIfChange[] changess, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        
        foreach(var item in items)
        {
            if(item.meterName == "Uptime SLA")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
