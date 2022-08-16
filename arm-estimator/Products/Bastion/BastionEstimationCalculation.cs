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
            if (item.meterName == "Basic Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Standard Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Standard Additional Gateway")
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
