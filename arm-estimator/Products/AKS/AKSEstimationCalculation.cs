using ACE.Calculation;
using ACE.WhatIf;
using System.Text.Json;

internal class AKSEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AKSEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var properties = JsonSerializer.Deserialize<AKSProperties>(JsonSerializer.Serialize(this.change.properties));

        foreach (var item in items)
        {
            double? cost = 0;

            if (item.meterName == "Uptime SLA")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.productName != null && item.productName.Contains("Virtual Machine"))
            {        
                if (properties != null && properties.AgentPoolProfiles != null)
                {
                    foreach (var pool in properties.AgentPoolProfiles)
                    {
                        VirtualMachineQueryFilter.DefineVmParameteres(pool.OsType!, pool.VmSize!, out var productName, out var skuName);
                        if(productName == item.productName && skuName == item.skuName)
                        {
                            var agentCount = pool.Count;
                            cost = item.retailPrice * HoursInMonth * agentCount;

                            // When there's more than a single agent pool, once found,
                            // break the loop so another item can be processed
                            break;
                        }
                    }
                }
            }
            else if (item.productName != null && item.productName.Contains("Managed Disk"))
            {
                if (properties != null && properties.AgentPoolProfiles != null)
                {
                    foreach (var pool in properties.AgentPoolProfiles)
                    {
                        var diskType = VirtualMachineQueryFilter.DetermineDiskType(pool.VmSize!);
                        if (diskType == item.productName)
                        {
                            var agentCount = pool.Count;
                            cost = item.retailPrice * agentCount;

                            // When there's more than a single agent pool, once found,
                            // break the loop so another item can be processed
                            break;
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
