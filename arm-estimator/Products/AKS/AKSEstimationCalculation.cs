using Azure.Core;
using System.Text.Json;

internal class AKSEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AKSEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var summary = new TotalCostSummary();

        foreach (var item in items)
        {
            double? cost = 0;

            if (item.meterName == "Uptime SLA")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.productName != null && item.productName.Contains("Virtual Machine"))
            {
                var properties = JsonSerializer.Deserialize<AKSProperties>(JsonSerializer.Serialize(this.change.properties));
                if (properties != null && properties.AgentPoolProfiles != null)
                {
                    foreach (var pool in properties.AgentPoolProfiles)
                    {
                        VirtualMachineQueryFilter.DefineVmParameteres(pool.OsType!, pool.VmSize!, out var productName, out var skuName);
                        if(productName == item.productName)
                        {
                            var agentCount = pool.Count;
                            cost = item.retailPrice * HoursInMonth * agentCount;
                        }
                    }
                }
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
