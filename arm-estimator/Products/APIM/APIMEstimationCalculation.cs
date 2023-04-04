using ACE.Calculation;
using ACE.WhatIf;

internal class APIMEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public APIMEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        int? capacity = GetCapacity();
        var items = GetItems();
        var summary = new TotalCostSummary();

        foreach (var item in items)
        {
            double? cost = 0;

            if (item.meterName == "Basic Unit")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Standard Unit")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Developer Unit")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Premium Unit")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Secondary Unit")
            {
                if (capacity >= 1)
                {
                    cost = item.retailPrice * HoursInMonth;
                }
            }
            else if (item.meterName == "Gateway Unit")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else
            {
                cost = item.retailPrice;
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

    private int? GetCapacity()
    {
        if(this.change.sku != null)
        {
            return this.change.sku?.capacity;
        }
        
        if(this.change.properties != null && this.change.properties.ContainsKey("sku_name"))
        {
            var sku = this.change.properties["sku_name"]!.ToString();
            if(sku != null)
            {
                return int.Parse(sku.Split('_')[1]);
            }
        }

        return null;
    }
}
