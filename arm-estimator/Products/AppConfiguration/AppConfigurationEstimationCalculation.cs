internal class AppConfigurationEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AppConfigurationEstimationCalculation(RetailItem[] items, WhatIfAfterBeforeChange change)
        : base(items, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        var consumptionMetrics = this.items.Where(_ => _.type != "Reservation");

        return this.items.Where(_ => _.type != "Reservation").OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost()
    {
        double? estimatedCost = 0;
        var items = GetItems();

        foreach (var item in items)
        {
            // Standard Instances
            if (item.meterId == "d1c90653-fe4f-4f50-9222-7032c35623b1")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
