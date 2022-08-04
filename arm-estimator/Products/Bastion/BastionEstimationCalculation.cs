using Azure.Core;

internal class BastionEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public BastionEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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

        var scaleUnits = 0;
        if(this.change.properties != null)
        {
            var scaleUnitsData = this.change.properties["scaleUnits"]?.ToString();
            if(scaleUnitsData != null)
            {
                scaleUnits = int.Parse(scaleUnitsData);
            }
        }

        foreach (var item in items)
        {
            // Basic Gateway
            if (item.meterId == "c6d29001-2ec1-43ce-8ec8-8c753399ff6c")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // Standard Gateway
            else if (item.meterId == "4836590f-078a-5184-b23a-63ad1c7979cc")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // Standard Additional Gateway
            else if (item.meterId == "bd08452f-cf67-539c-baa1-1ef7d6fb13d0")
            {
                estimatedCost += item.retailPrice * HoursInMonth * scaleUnits;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
