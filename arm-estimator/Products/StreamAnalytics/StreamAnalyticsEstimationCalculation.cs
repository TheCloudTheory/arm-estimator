using ACE.Calculation;
using ACE.WhatIf;
using Azure.Core;
using System.Text.Json;

internal class StreamAnalyticsEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public StreamAnalyticsEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        int? capacity = 1;
        var summary = new TotalCostSummary();

        if (this.change.type != null && this.change.type == "Microsoft.StreamAnalytics/clusters")
        {
            capacity = this.change?.sku?.capacity;
        }

        if (this.change?.type != null && this.change.type == "Microsoft.StreamAnalytics/streamingjobs")
        {
            if(this.change.properties != null && this.change.properties.ContainsKey("transformation"))
            {
                var transformation = ((JsonElement)this.change.properties["transformation"]).Deserialize<StreamAnalyticsJobTransformation>();
                if(transformation != null)
                {
                    capacity = transformation.properties?.streamingUnits;
                }
            }
        }

        foreach (var item in items)
        {
            var cost = item.retailPrice * HoursInMonth * capacity;

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
