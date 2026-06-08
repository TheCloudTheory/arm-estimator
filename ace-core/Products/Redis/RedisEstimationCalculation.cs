using ACE.Calculation;
using ACE.WhatIf;
using System.Text.Json;

internal class RedisEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public RedisEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change, double conversionRate)
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
        var skuData = ((JsonElement)this.change.properties!["sku"]!).Deserialize<RedisSkuData>();
        var shardCount = 1;
        var summary = new TotalCostSummary();

        if (skuData != null && skuData.family == "P")
        {
            if (this.change.properties != null && this.change.properties.ContainsKey("shardCount"))
            {
                _ = int.TryParse(this.change.properties["shardCount"]!.ToString(), out shardCount);
            }
        }

        var replicas = 0;

        if (this.change.properties != null)
        {
            if (this.change.properties.TryGetValue("replicasPerMaster", out var replicasCount))
            {
                replicas = int.Parse(replicasCount!.ToString()!);
            }
            else if (this.change.properties.TryGetValue("replicasPerMaster", out replicasCount))
            {
                replicas = int.Parse(replicasCount!.ToString()!);
            }
        }

        foreach (var item in items)
        {
            double? cost = 0;

            if(item.meterName != null && item.meterName.Contains("Cache Instance"))
            {
                cost = item.retailPrice * HoursInMonth * shardCount * replicas;
            }
            else
            {
                cost = item.retailPrice * HoursInMonth * shardCount;
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
