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

    public TotalCostSummary GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        var summary = new TotalCostSummary();

        foreach (var item in items)
        {
            double? cost = 0;

            if(item.meterId == "vCPU Active Usage")
            {
                cost = item.retailPrice * HoursInMonth / 2 * 3600;
            }
            else if (item.meterId == "Memory Active Usage")
            {
                cost = item.retailPrice * HoursInMonth / 2 * 3600;
            }
            else if (item.meterId == "vCPU Idle Usage")
            {
                cost = item.retailPrice * HoursInMonth / 2 * 3600;
            }
            else if (item.meterId == "Memory Idle Usage")
            {
                cost = item.retailPrice * HoursInMonth / 2 * 3600;
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
