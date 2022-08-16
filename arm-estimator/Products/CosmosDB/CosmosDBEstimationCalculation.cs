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

    public double GetTotalCost(WhatIfChange[] changes)
    {
        double? estimatedCost = 0;
        var rus = 0;
        var items = GetItems();

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
            if (item.meterName == "100 RU/s")
            {
                estimatedCost += item.retailPrice * HoursInMonth * rus;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
