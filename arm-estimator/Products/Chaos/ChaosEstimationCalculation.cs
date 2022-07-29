internal class ChaosEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public ChaosEstimationCalculation(RetailItem[] items, WhatIfAfterBeforeChange change)
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
            // Assumption - if one uses Chaos Studio, let's say they're leveraging
            // its capabilities each day for 15 minutes
            estimatedCost += item.retailPrice * 15 * 30;
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
