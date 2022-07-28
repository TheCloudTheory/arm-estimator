internal class AnalysisServicesEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AnalysisServicesEstimationCalculation(RetailItem[] items, WhatIfAfterBeforeChange change)
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
        int? capacity = 1;

        if(change.sku != null)
        {
            if(change.sku.capacity != null)
            {
                capacity = change.sku.capacity;
            }
        }

        foreach (var item in items)
        {
            // S0 scale-out
            if(item.meterId == "d47ce8ce-3cdc-402a-8166-9e2c1ecda823")
            {
                if(capacity > 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S1 scale-out
            else if (item.meterId == "41669c0a-0d33-4242-a53a-3ba563e8c39e")
            {
                if (capacity > 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S2 scale-out
            else if (item.meterId == "a17a42a4-12c0-47e5-920b-6d1d30b476a7")
            {
                if (capacity > 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S4 scale-out
            else if (item.meterId == "c905af03-1e62-4ec2-9be0-2d8497afbe22")
            {
                if (capacity > 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S8 scale-out
            else if (item.meterId == "bb2f59fa-700a-4a6d-9a5c-79183f8a0004")
            {
                if (capacity > 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S9 scale-out
            else if (item.meterId == "792303a3-8c90-47c7-b047-147eb321323a")
            {
                if (capacity > 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S8 V2 scale-out
            else if (item.meterId == "8ac6ef2a-07cc-4451-a9f6-f4c3ffb41c06")
            {
                if (capacity > 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth * capacity;
                }
            }
            // S9 V2 scale-out
            else if (item.meterId == "792303a3-8c90-47c7-b047-147eb321323a")
            {
                if (capacity > 1)
                {
                    estimatedCost += item.retailPrice * HoursInMonth * capacity;
                }
            }
            else
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
