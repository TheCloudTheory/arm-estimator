using ACE.Calculation;
using ACE.WhatIf;

internal class SQLEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    private const double HybridBenefitCost = 145.95d;
    private const double HybridBenefitCost_BusinessCritical = 547.50d;

    public SQLEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change, double conversionRate)
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

            if(item.meterName == "B DTU")
            {
                cost = item.retailPrice * 30;
            }
            else if (item.meterName == "S0 DTUs" || 
                item.meterName == "S1 DTUs" || 
                item.meterName == "S2 DTUs" || 
                item.meterName == "S3 DTUs" || 
                item.meterName == "S4 DTUs" ||
                item.meterName == "10 DTUs" ||
                item.meterName == "P1 DTU" ||
                item.meterName == "P2 DTU" ||
                item.meterName == "P4 DTU" ||
                item.meterName == "P6 DTU" ||
                item.meterName == "P11 DTU" ||
                item.meterName == "P15 DTU")
            {
                cost = item.retailPrice * (730 / 24d);
            }
            else if (item.meterName == "vCore" )
            {
                cost = item.retailPrice * HoursInMonth + AddHybridBenefitCostIfNeeded(this.change.sku!.name!, usagePatterns);
            }
            else if (item.meterName == "Zone Redundancy vCore")
            {
                cost = item.retailPrice * HoursInMonth * 2.6667 + AddHybridBenefitCostIfNeeded(this.change.sku!.name!, usagePatterns);
            }
            else if (item.meterName == "Data Stored")
            {
                if(item.skuName == "Standard")
                {
                    if(usagePatterns != null && usagePatterns.ContainsKey("Microsoft_Sql_servers_databases_DTU_Storage"))
                    {
                        var definedSize = int.Parse(usagePatterns["Microsoft_Sql_servers_databases_DTU_Storage"]);
                        cost = item.retailPrice * definedSize - item.retailPrice * 250;
                    }              
                }

                if (item.skuName == "Premium")
                {
                    if (usagePatterns != null && usagePatterns.ContainsKey("Microsoft_Sql_servers_databases_DTU_Storage"))
                    {
                        var definedSize = int.Parse(usagePatterns["Microsoft_Sql_servers_databases_DTU_Storage"]);
                        cost = item.retailPrice * definedSize - item.retailPrice * 500;
                    }
                }
            }
            else if (item.meterName == "General Purpose Data Stored")
            {
                if (usagePatterns != null && usagePatterns.ContainsKey("Microsoft_Sql_servers_databases_vCore_Storage"))
                {
                    var definedSize = int.Parse(usagePatterns["Microsoft_Sql_servers_databases_vCore_Storage"]);
                    cost = (item.retailPrice * definedSize) + (item.retailPrice * definedSize * 0.3); // Includes average log size
                }
                else
                {
                    cost = item.retailPrice;
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

    private double AddHybridBenefitCostIfNeeded(string skuName, IDictionary<string, string>? usagePatterns)
    {
        if(base.IncludeUsagePattern("Microsoft_Sql_servers_Hybrid_Benefit_Enabled", usagePatterns, 0) != 0)
        {
            return 0;
        }

        var rawCost = skuName.StartsWith("BC_") ? HybridBenefitCost_BusinessCritical : HybridBenefitCost;

        return rawCost * this.conversionRate * SQLQueryFilter.GetNumberOfCoresBasedOnSku(skuName) / 2;
    }
}
