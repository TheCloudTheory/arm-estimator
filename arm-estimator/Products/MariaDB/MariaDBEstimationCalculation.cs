using ACE.Calculation;
using ACE.WhatIf;
using Azure.Core;
using System.Text.Json;

internal class MariaDBEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public MariaDBEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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

            if (item.meterName == "vCore")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Data Stored"
                || item.meterName == "General Purpose Data Stored"
                || item.meterName == "Perf Optimized Data Stored"
                || item.meterName == "Basic Data Stored")
            {
                if (this.change.sku?.size != null)
                {
                    var sizeInGbs = double.Parse(this.change.sku.size) / 1024;
                    cost = item.retailPrice * sizeInGbs;
                }
                else
                {
                    cost = item.retailPrice;
                }
            }
            else if (item.meterName == "Backup LRS Data Stored" || item.meterName == "Backup GRS Data Stored")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_DBforMariaDB_servers_Backup_Storage", usagePatterns, 0);
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
