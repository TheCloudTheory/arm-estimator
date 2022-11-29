using Azure.Core;

internal class HealthBotServiceEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public HealthBotServiceEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();

        foreach (var item in items)
        {
            if (item.meterName == "Standard Unit")
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
