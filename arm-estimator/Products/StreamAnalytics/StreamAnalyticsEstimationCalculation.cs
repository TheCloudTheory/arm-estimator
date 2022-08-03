using Azure.Core;
using System.Text.Json;

internal class StreamAnalyticsEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public StreamAnalyticsEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        var consumptionMetrics = this.items.Where(_ => _.type != "Reservation");

        return this.items.Where(_ => _.type != "Reservation").OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changes)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        int? capacity = 1;

        if(this.change.type != null && this.change.type == "Microsoft.StreamAnalytics/clusters")
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
            estimatedCost += item.retailPrice * HoursInMonth * capacity;
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
