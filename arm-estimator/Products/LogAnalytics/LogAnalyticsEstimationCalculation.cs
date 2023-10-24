using ACE.Calculation;
using ACE.WhatIf;
using System.Text.Json;

internal class LogAnalyticsEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public LogAnalyticsEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change, double conversionRate)
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
        var summary = new TotalCostSummary();

        double? dailyQuota = null;
        if (this.change.properties != null && this.change.properties.ContainsKey("workspaceCapping"))
        {
            var cappingProperties = ((JsonElement)this.change.properties["workspaceCapping"]!).Deserialize<WorkspaceCapping>();
            dailyQuota = cappingProperties?.DailyQuotaGB;
        }
        
        foreach (var item in items)
        {
            double? cost = 0;

            if (item.meterName == "Standard Instances")
            {
                cost = item.retailPrice * 30;
            }
            else if (item.meterName == "Pay-as-you-go Data Analyzed" && dailyQuota != null)
            {
                // Remember, that first 5GBs are free
                var baseCost = item.retailPrice * 30 * dailyQuota - (5 * item.retailPrice);
                cost = baseCost < 0 ? 0 : baseCost;
            }
            else if (item.meterName == "Pay-as-you-go Data Analyzed" && dailyQuota == null)
            {
                // Remember, that first 5GBs are free
                var baseCost = item.retailPrice * base.IncludeUsagePattern("Microsoft_OperationalInsights_workspaces_Paug_Data_Ingestion", usagePatterns, 1) - (5 * item.retailPrice);
                cost = baseCost < 0 ? 0 : baseCost;
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
