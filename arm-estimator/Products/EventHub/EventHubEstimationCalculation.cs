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

    public TotalCostSummary GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        var capacity = this.change.sku?.capacity == null ? 1 : this.change.sku?.capacity;
        var summary = new TotalCostSummary();

        foreach (var item in items)
        {
            double? cost = 0;

            if (item.meterName == "Basic Throughput Unit")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Standard Throughput Unit")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Processing Unit")
            {
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Capture" && this.change.type == "Microsoft.EventHub/namespaces/eventhubs")
            {
                var parentId = this.id.Parent?.ToString();
                var parent = changes.Single(_ => _.resourceId == parentId);
                var parentDesiredState = parent.after ?? parent.before;

                capacity = parentDesiredState?.sku?.capacity;
                cost = item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Dedicated Capacity Unit")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else
            {
                cost = item.retailPrice;
            }

            estimatedCost += cost;
            summary.DetailedCost.Add(item.meterName!, cost);
        }

        return summary;
    }
}
