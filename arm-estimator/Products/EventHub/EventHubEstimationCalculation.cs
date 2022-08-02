internal class EventHubEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public EventHubEstimationCalculation(RetailItem[] items, WhatIfAfterBeforeChange change)
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
        var capacity = this.change.sku?.capacity == null ? 1 : this.change.sku?.capacity;

        foreach (var item in items)
        {
            // Basic TU
            if (item.meterId == "177ee643-5434-4c04-ae11-9e01672ed87e")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Standard TU
            else if (item.meterId == "62d94a65-9300-48a6-8c15-0e70fc41eb44")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Processing Unit
            else if (item.meterId == "49f32255-3f62-545f-92fe-ea7d3cb35088")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Capture
            else if (item.meterId == "36085934-4216-4d15-a257-9670b5eb12dc")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Dedicated Capacity Unit
            else if (item.meterId == "61e1c624-5f16-4db3-9da2-991e72892d67")
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
