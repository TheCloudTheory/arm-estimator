internal class CognitiveSearchEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public CognitiveSearchEstimationCalculation(RetailItem[] items, WhatIfAfterBeforeChange change)
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
            estimatedCost += item.retailPrice * HoursInMonth;
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
