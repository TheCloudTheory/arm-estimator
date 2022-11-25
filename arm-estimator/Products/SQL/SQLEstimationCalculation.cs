using Azure.Core;

internal class SQLEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public SQLEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        
        foreach(var item in items)
        {
            if(item.meterName == "B DTU")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterName == " S0 DTUs")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterName == "S1 DTUs")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterName == "S2 DTUs")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterName == "S3 DTUs")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterName == "10 DTU")
            {
                estimatedCost +=  item.retailPrice * 30;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
