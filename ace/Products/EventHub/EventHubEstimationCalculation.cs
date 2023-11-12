using ACE.Calculation;
using ACE.WhatIf;

internal class EventHubEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public EventHubEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change, double conversionRate)
        : base(items, id, change, conversionRate)
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
                var parentId = this.id.GetParent()?.ToString();
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
            if (summary.DetailedCost.ContainsKey(item.meterName!))
            {
                summary.DetailedCost[item.meterName!] += cost;
            }
            else
            {
                summary.DetailedCost.Add(item.meterName!, cost);
            }
        }

        summary.TotalCost = estimatedCost.GetValueOrDefault();
        return summary;
    }
}
