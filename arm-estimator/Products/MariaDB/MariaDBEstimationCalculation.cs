using Azure.Core;
using System.Text.Json;

internal class MariaDBEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public MariaDBEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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

        foreach (var item in items)
        {
            if (item.meterName == "vCore")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Data Stored"
                || item.meterName == "General Purpose Data Stored"
                || item.meterName == "Perf Optimized Data Stored")
            {
                if (this.change.sku?.size != null)
                {
                    var sizeInGbs = double.Parse(this.change.sku.size) / 1024;
                    estimatedCost += item.retailPrice * sizeInGbs;
                }
                else
                {
                    estimatedCost += item.retailPrice;
                }
            }
            else if (item.meterName == "Backup LRS Data Stored" || item.meterName == "Backup GRS Data Stored")
            {
                var storageProfile = ((JsonElement)this.change.properties!["storageProfile"]).Deserialize<MariaDBStorageProfile>();
                if (storageProfile != null)
                {
                    if (this.change.sku?.size != null && storageProfile.storageMB > int.Parse(this.change.sku?.size!))
                    {
                        var mbsDifference = storageProfile.storageMB - int.Parse(this.change.sku?.size!);
                        var sizeInGbs = mbsDifference / 1024d;
                        estimatedCost += item.retailPrice * sizeInGbs;
                    }
                }
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
