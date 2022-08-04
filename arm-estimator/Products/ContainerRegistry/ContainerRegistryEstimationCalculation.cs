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

    public double GetTotalCost(WhatIfChange[] changes)
    {
        double? estimatedCost;
        var items = GetItems();
        
        var basicRegistryUnitMetric = items.SingleOrDefault(_ => _.meterId == "5c9e7a65-5784-494c-9718-7749d4075dd9");
        var standardRegistryUnitMetric = items.SingleOrDefault(_ => _.meterId == "2f8e1105-47d0-4105-a934-b2175c766a71");
        var premiumRegistryUnitMetric = items.SingleOrDefault(_ => _.meterId == "984c39d8-2499-4233-8a97-fb002a809f8f");
        if (basicRegistryUnitMetric != null)
        {
            estimatedCost = items.Where(_ => _.meterId != "5c9e7a65-5784-494c-9718-7749d4075dd9").Select(_ => _.retailPrice).Sum();
            estimatedCost += basicRegistryUnitMetric.retailPrice * 30;
        }
        else if (standardRegistryUnitMetric != null)
        {
            estimatedCost = items.Where(_ => _.meterId != "2f8e1105-47d0-4105-a934-b2175c766a71").Select(_ => _.retailPrice).Sum();
            estimatedCost += standardRegistryUnitMetric.retailPrice * 30;
        }
        else if (premiumRegistryUnitMetric != null)
        {
            estimatedCost = items.Where(_ => _.meterId != "984c39d8-2499-4233-8a97-fb002a809f8f").Select(_ => _.retailPrice).Sum();
            estimatedCost += premiumRegistryUnitMetric.retailPrice * 30;
        }
        else
        {
            estimatedCost = items.Select(_ => _.retailPrice).Sum();
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
