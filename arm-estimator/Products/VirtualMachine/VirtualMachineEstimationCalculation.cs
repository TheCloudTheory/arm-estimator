using Azure.Core;

internal class VirtualMachineEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public VirtualMachineEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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

        foreach (var item in items)
        {
            // A0 Basic Windows
            if (item.meterId == "544d011e-8ed2-4e78-a99e-d87c0f5d5f19")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // A1 Basic Windows
            else if (item.meterId == "fee1c571-5e0d-47ef-b031-3ba67f6a67c2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // A2 Basic Windows
            else if (item.meterId == "7cb2e14f-ff57-436b-b155-3d3787649979")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // A3 Basic Windows
            else if (item.meterId == "26586f97-b6fd-4bb5-9d05-1e11493cf746")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // A4 Basic Windows
            else if (item.meterId == "78a15035-8e38-47a4-bf17-bed40e6235b2")
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
