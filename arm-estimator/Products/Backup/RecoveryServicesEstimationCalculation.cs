﻿using Azure.Core;

internal class RecoveryServicesEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public RecoveryServicesEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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

            if (item.meterName == "Standard Instances")
            {
                cost = item.retailPrice * 30;
            }
            else if (item.meterName == "LRS Data Stored" ||
                item.meterName == "ZRS Data Stored" ||
                item.meterName == "GRS Data Stored" ||
                item.meterName == "RA-GRS Data Stored")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_RecoveryServices_vaults_Data_Stored", usagePatterns);
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
