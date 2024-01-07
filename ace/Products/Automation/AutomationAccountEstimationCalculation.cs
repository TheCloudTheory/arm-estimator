using ACE.Calculation;
using ACE.WhatIf;

internal class AutomationAccountEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AutomationAccountEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change, double conversionRate)
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

        foreach (var item in items)
        {
            double? cost = 0;

            if (item.meterName == "Watcher")
            {
                cost = item.retailPrice * HoursInMonth * base.IncludeUsagePattern("Microsoft_Automation_automationAccounts_Watcher_Count", usagePatterns);
            }
            else if (item.meterName == "Basic Runtime")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_Automation_automationAccounts_Basic_Runtime", usagePatterns);
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
