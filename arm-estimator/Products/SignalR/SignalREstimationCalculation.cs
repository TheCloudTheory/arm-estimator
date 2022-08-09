using Azure.Core;

internal class SignalREstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public SignalREstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var capacity = this.change.sku?.capacity;
        
        if(capacity == null)
        {
            capacity = 1;
        }

        foreach (var item in items)
        {
            // Units - Standard
            if (item.meterId == "629ccd49-5991-5138-b87d-a800120c44f1")
            {
                estimatedCost += item.retailPrice * 30 * capacity;
            }
            // Units - Premium
            else if (item.meterId == "9706e6a9-3463-4a29-bcd2-7cb69683bff2")
            {
                estimatedCost += item.retailPrice * 30 * capacity;
            }
            else
            {
                estimatedCost += item.retailPrice * capacity;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
