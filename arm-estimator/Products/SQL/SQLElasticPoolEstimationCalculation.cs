using ACE.Calculation;
using ACE.WhatIf;

internal class SQLElasticPoolEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    private const double HybridBenefitCost = 145.95d;

    public SQLElasticPoolEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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

            if(item.meterName == "eDTUs")
            {
                cost = item.retailPrice * 30;
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

    private double AddHybridBenefitCostIfNeeded(string skuName, IDictionary<string, string>? usagePatterns)
    {
        if(base.IncludeUsagePattern("Microsoft_Sql_servers_Hybrid_Benefit_Enabled", usagePatterns, 0) != 0)
        {
            return 0;
        }

        return HybridBenefitCost * SQLQueryFilter.GetNumberOfCoresBasedOnSku(skuName) / 2;
    }
}
