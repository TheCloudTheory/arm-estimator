using Azure.Core;

internal class LogicAppsEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public LogicAppsEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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

        foreach (var item in items)
        {
            // Base Unit
            if (item.meterId == "311aadf2-331e-43ec-93e2-457e32a109bc")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // Base Unit
            else if (item.meterId == "516c2647-5453-46ab-a261-a360cc3a8f70")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
