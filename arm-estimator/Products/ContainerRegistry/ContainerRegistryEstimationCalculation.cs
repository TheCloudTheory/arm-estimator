using Azure.Core;

internal class ContainerRegistryEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public ContainerRegistryEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changess, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        
        foreach(var item in items)
        {
            if (item.meterId == "Basic Registry Unit")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterId == "Standard Registry Unit")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterId == "Premium Registry Unit")
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
