using Azure.Core;

internal class ApplicationInsightsEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public ApplicationInsightsEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        var consumptionMetrics = this.items.Where(_ => _.type != "Reservation");

        return this.items.Where(_ => _.type != "Reservation")
            .Where(_ => _.meterId != "2073b0aa-c836-4642-9d97-0635f52e3520")
            .Where(_ => _.meterId != "e5626349-2f1f-4563-babc-dc9219884716")
            .Where(_ => _.meterId != "127fa4bc-00f0-4ffe-b884-f3fcc5e62790")
            .OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changes)
    {
        double? estimatedCost = 0;
        var items = GetItems();

        foreach (var item in items)
        {
            // Pay-as-you-go Data Analyzed
            if (item.meterId == "8c945adb-3f9a-40d1-a661-efc36fa4a3e0")
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
