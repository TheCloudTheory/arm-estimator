using Azure.Core;

internal class APIMEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public APIMEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var capacity = this.change.sku?.capacity;
        var items = GetItems();

        foreach (var item in items)
        {
            if(item.meterName == "Basic Units")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Standard Units")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Developer Units")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Premium Units")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Secondary Units")
            {
                if(capacity >= 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth;
                }
            }
            else if (item.meterName == "Gateway Units")
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
