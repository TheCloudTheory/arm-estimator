using Azure.Core;

internal class StorageAccountEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public StorageAccountEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost;
        var items = GetItems();
        
        estimatedCost = items.Select(_ => _.retailPrice).Sum();
        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
