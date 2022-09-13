using Azure.Core;
using System.Text.Json;

internal class RedisEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public RedisEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var items = GetItems();
        var skuData = ((JsonElement)this.change.properties!["sku"]).Deserialize<RedisSkuData>();
        var shardCount = 1;

        if (skuData != null && skuData.family == "P")
        {
            if (this.change.properties != null && this.change.properties.ContainsKey("shardCount"))
            {
                _ = int.TryParse(this.change.properties["shardCount"].ToString(), out shardCount);
            }
        }

        foreach (var item in items)
        {
            estimatedCost += item.retailPrice * HoursInMonth * shardCount;
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
