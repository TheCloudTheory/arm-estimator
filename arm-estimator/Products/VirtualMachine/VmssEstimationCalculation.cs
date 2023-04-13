using ACE.Calculation;
using ACE.WhatIf;

internal class VmssEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public VmssEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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
            double? cost;
            if(item.productName != null && item.productName.Contains("Managed Disk"))
            {
                cost = item.retailPrice * this.change.sku!.capacity;
            }
            else
            {
                cost = item.retailPrice * HoursInMonth * this.change.sku!.capacity;
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
