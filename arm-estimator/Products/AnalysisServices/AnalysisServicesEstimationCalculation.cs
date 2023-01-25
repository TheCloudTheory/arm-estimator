using ACE.Calculation;
using ACE.WhatIf;
using Azure.Core;

internal class AnalysisServicesEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AnalysisServicesEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        int? capacity = 1;
        var summary = new TotalCostSummary();

        if (change.sku != null)
        {
            if(change.sku.capacity != null)
            {
                capacity = change.sku.capacity;
            }
        }

        foreach (var item in items)
        {
            double? cost = 0;

            if (item.meterName == "S0 Scale-Out")
            {
                if(capacity >= 1)
                {
                    cost = item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S1 scale-out
            else if (item.meterName == "S1 Scale-Out")
            {
                if (capacity >= 1)
                {
                    cost = item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S2 scale-out
            else if (item.meterName == "S2 Scale-Out")
            {
                if (capacity >= 1)
                {
                    cost = item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S4 scale-out
            else if (item.meterName == "S4 Scale-Out")
            {
                if (capacity >= 1)
                {
                    cost = item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S8 scale-out
            else if (item.meterName == "S8 Scale-Out")
            {
                if (capacity >= 1)
                {
                    cost = item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S9 scale-out
            else if (item.meterName == "S9 Scale-Out")
            {
                if (capacity >= 1)
                {
                    cost = item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S8 V2 scale-out
            else if (item.meterName == "S8 v2 Scale-Out")
            {
                if (capacity >= 1)
                {
                    cost = item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S9 V2 scale-out
            else if (item.meterName == "S9 v2 Scale-Out")
            {
                if (capacity >= 1)
                {
                    cost = item.retailPrice * HoursInMonth * capacity;
                }
            }
            else
            {
                cost = item.retailPrice * HoursInMonth;
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
