using Azure.Core;

internal class EventHubEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public EventHubEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var capacity = this.change.sku?.capacity == null ? 1 : this.change.sku?.capacity;

        foreach (var item in items)
        {
            if (item.meterName == "Basic Throughput Unit")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Standard Throughput Unit")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Processing Unit")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Capture" && this.change.type == "Microsoft.EventHub/namespaces/eventhubs")
            {
                var parentId = this.id.Parent?.ToString();
                var parent = changes.Single(_ => _.resourceId == parentId);
                var parentDesiredState = parent.after ?? parent.before;

                capacity = parentDesiredState?.sku?.capacity;
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Dedicated Capacity Unit")
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
