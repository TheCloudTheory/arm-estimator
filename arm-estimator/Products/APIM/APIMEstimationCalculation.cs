using Azure.Core;

internal class APIMEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public APIMEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        var consumptionMetrics = this.items.Where(_ => _.type != "Reservation");

        return this.items.Where(_ => _.type != "Reservation").OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changes)
    {
        double? estimatedCost = 0;
        var capacity = this.change.sku?.capacity;
        var items = GetItems();

        foreach (var item in items)
        {
            // Basic Units
            if(item.meterId == "24add188-4886-4ae1-a78c-49a974027be0")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Standard Units
            else if (item.meterId == "2e99cfc3-341a-4699-8f9a-38498d008def")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Developer Units
            else if (item.meterId == "17c7c650-0e4a-4a4f-99d8-412d1ebcb892")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Premium Units
            else if (item.meterId == "15ebd8a7-aeb0-4623-ac0f-d434504f9474")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // Secondary Units
            else if (item.meterId == "ad75eb29-e062-5b73-a26a-47b97891cc24")
            {
                if(capacity > 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth;
                }
            }
            // Gateway Units
            else if (item.meterId == "d42b8719-acfb-4de6-88c7-6fcf83338722")
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
