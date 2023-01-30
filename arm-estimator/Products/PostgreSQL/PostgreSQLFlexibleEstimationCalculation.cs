using ACE.Calculation;
using ACE.WhatIf;
using System.Text.Json;

internal class PostgreSQLFlexibleEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public PostgreSQLFlexibleEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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

            if (item.meterName != null && (item.meterName.Contains("vCore") || item.meterName == "B1MS" || item.meterName == "B2S"))
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Basic Data Stored"
                || item.meterName == "General Purpose Data Stored"
                || item.meterName == "Perf Optimized Data Stored")
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
                if(this.change.properties != null && this.change.properties.ContainsKey("storageProfile"))
                {
                    var storageProfile = ((JsonElement)this.change.properties["storageProfile"]).Deserialize<PostgreSQLStorageProfile>();
                    if(storageProfile?.geoRedundantBackup == "Enabled")
                    {
                        cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_DBforPostgreSQL_servers_Backup_Storage", usagePatterns, 0) * 2;
                    }
                    else
                    {
                        cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_DBforPostgreSQL_servers_Backup_Storage", usagePatterns, 0);
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
