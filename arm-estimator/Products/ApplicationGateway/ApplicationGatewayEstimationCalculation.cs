using Azure.Core;
using System.Text.Json;

internal class ApplicationGatewayEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public ApplicationGatewayEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
            if (item.meterName == "Small Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Medium Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Large Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == " Capacity Units")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Fixed Cost")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Medium Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Large Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
