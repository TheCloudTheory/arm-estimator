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

    public double GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
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

        var replicas = 0;

        if (this.change.properties != null)
        {
            if (this.change.properties.TryGetValue("replicasPerMaster", out var replicasCount))
            {
                replicas = int.Parse(replicasCount.ToString()!);
            }
            else if (this.change.properties.TryGetValue("replicasPerMaster", out replicasCount))
            {
                replicas = int.Parse(replicasCount.ToString()!);
            }
        }

        foreach (var item in items)
        {
            if(item.meterName != null && item.meterName.Contains("Cache Instance"))
            {
                estimatedCost += item.retailPrice * HoursInMonth * shardCount * replicas;
            }
            else
            {
                estimatedCost += item.retailPrice * HoursInMonth * shardCount;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
