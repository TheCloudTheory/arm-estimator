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

    public TotalCostSummary GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        var summary = new TotalCostSummary();

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
            double? cost = 0;

            if (item.meterName == "Basic Gateway")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Standard Gateway")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Standard Additional Gateway")
            {
                cost = item.retailPrice * HoursInMonth * scaleUnits;
            }
            else
            {
                cost = item.retailPrice;
            }

            estimatedCost += cost;
            summary.DetailedCost.Add(item.meterName!, cost);
        }

        return summary;
    }
}
