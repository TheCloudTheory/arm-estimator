using Azure.Core;
using System.Text.Json;

internal class CosmosDBEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public CosmosDBEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var rus = 0;
        var items = GetItems();
        var summary = new TotalCostSummary();

        if (this.change.properties != null && this.change.properties.ContainsKey("options"))
        {
            var options = this.change.properties["options"];
            if (options != null)
            {
                var data = ((JsonElement)options).Deserialize<Dictionary<string, object>>();
                if (data != null && data.ContainsKey("throughput"))
                {
                    var throughput = data["throughput"]?.ToString();
                    if (throughput != null)
                    {
                        rus = int.Parse(throughput) / 100;
                    }
                }
            }
        }

        foreach (var item in items)
        {
            double? cost = 0;

            if (item.meterName == "100 RU/s")
            {
                cost = item.retailPrice * HoursInMonth * rus;
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
