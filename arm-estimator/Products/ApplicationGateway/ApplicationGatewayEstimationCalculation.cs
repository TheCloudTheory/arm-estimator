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
            // Small Gateway
            if (item.meterId == "a0e7e224-c315-436c-a320-b862cf9641fa")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Medium Gateway
            else if (item.meterId == "cc2a802e-6a8d-47d8-a6aa-cecef8c6b0ec")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Large Gateway
            else if (item.meterId == "72d4a40c-5355-484c-b1bc-6f746466bfdf")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Standard V2 - Capacity Units
            else if (item.meterId == "49bc1895-a428-4065-8128-ed6d1f198677")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Standard V2 - Fixed Cost
            else if (item.meterId == "c0ce9247-e2a8-479c-bf2e-0499e720ae0d")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // Medium WAF
            else if (item.meterId == "f94e369c-ea49-4c72-b69c-e7f072327070")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Large WAF
            else if (item.meterId == "2962ce28-78d8-45bf-85ba-232032efd04a")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // WAF V2 - Fixed Cost
            else if (item.meterId == "5e47ce41-ebfb-4fc4-a5fe-07e14d518ac1")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // WAF V2 - Capacity Units
            else if (item.meterId == "78fe1aba-44ce-435b-b710-8c9babdb1a7d")
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
