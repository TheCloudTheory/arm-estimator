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
            // B DTUs
            if(item.meterId == "cae64797-9ecf-4906-b517-6238c80c045f")
            {
                estimatedCost += item.retailPrice * 30;
            }
            // S0 DTUs
            else if (item.meterId == "a149966f-73b4-4e1d-b335-d2a572b1e6bd")
            {
                estimatedCost += item.retailPrice * 30;
            }
            // S1 DTUs
            else if (item.meterId == "54c6bbf3-322b-406d-8411-101bc4b9443a")
            {
                estimatedCost += item.retailPrice * 30;
            }
            // S2 DTUs
            else if (item.meterId == "f303a21a-05f1-4563-b2ad-7523eebc4058")
            {
                estimatedCost += item.retailPrice * 30;
            }
            // S3 DTUs
            else if (item.meterId == "6bafdc11-b964-4895-9d4e-a0e548db1b2b")
            {
                estimatedCost += item.retailPrice * 30;
            }
            // 10 DTU - everything above S3
            else if (item.meterId == "94aaed62-bf2f-4461-8f69-078933cb2f72")
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
