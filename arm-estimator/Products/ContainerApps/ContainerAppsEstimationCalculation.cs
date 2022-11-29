using Azure.Core;

internal class ContainerAppsEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public ContainerAppsEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        
        foreach(var item in items)
        {
            if(item.meterId == "vCPU Active Usage")
            {
                estimatedCost += item.retailPrice * HoursInMonth / 2 * 3600;
            }
            else if (item.meterId == "Memory Active Usage")
            {
                estimatedCost += item.retailPrice * HoursInMonth / 2 * 3600;
            }
            else if (item.meterId == "vCPU Idle Usage")
            {
                estimatedCost += item.retailPrice * HoursInMonth / 2 * 3600;
            }
            else if (item.meterId == "Memory Idle Usage")
            {
                estimatedCost += item.retailPrice * HoursInMonth / 2 * 3600;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
