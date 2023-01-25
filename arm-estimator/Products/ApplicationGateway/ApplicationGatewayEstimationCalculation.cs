using ACE.Calculation;
using ACE.WhatIf;
using Azure.Core;
using System.Text.Json;

internal class ApplicationGatewayEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public ApplicationGatewayEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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

        var capacity = 1;
        if(this.change.properties != null && this.change.properties.ContainsKey("sku"))
        {
            var skuData = this.change.properties["sku"];
            if(skuData != null)
            {
                var sku = ((JsonElement)skuData).Deserialize<Dictionary<string, object>>();
                if(sku != null && sku.ContainsKey("capacity"))
                {
                    var skuCapacity = sku["capacity"]?.ToString();
                    if(skuCapacity != null)
                    {
                        capacity = int.Parse(skuCapacity);
                    }
                }
            }
        }

        foreach (var item in items)
        {
            double? cost = 0;

            if (item.meterName == "Small Gateway")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Medium Gateway")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Large Gateway")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Standard Capacity Units")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Standard Fixed Cost")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Medium Gateway")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Large Gateway")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
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
}
