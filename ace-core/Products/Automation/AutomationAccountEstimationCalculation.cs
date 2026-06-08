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
                var numberOfWatchers = base.IncludeUsagePattern("Microsoft_Automation_automationAccounts_Watcher_Count", usagePatterns);
                if(numberOfWatchers == 1) {
                    cost = 0;
                    continue;
                }

                // 744 hours of Watcher is free so we need to subtract that from the total
                cost = item.retailPrice * HoursInMonth * (numberOfWatchers - 1);
            }
            else if (item.meterName == "Basic Runtime")
            {
                var runningMinutes = base.IncludeUsagePattern("Microsoft_Automation_automationAccounts_Basic_Runtime", usagePatterns);
                if(runningMinutes <= 500) {
                    cost = 0;
                    continue;
                }

                cost = item.retailPrice * (runningMinutes - 500);
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
