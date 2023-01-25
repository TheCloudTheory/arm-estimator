using ACE.Calculation;
using ACE.WhatIf;

internal class RedisEnterpriseEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public static readonly IReadOnlyDictionary<string, double> LicenseCost = new Dictionary<string, double>()
    {
        { "E10", 273.02d },
        { "E20", 544.58d },
        { "E50", 1070.18d },
        { "E100", 2139.63d },
        { "F300", 3528.09d },
        { "F700", 7056.18d },
        { "F1500", 14111.63d }
    };

    public RedisEnterpriseEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var skuData = this.change.sku;

        foreach (var item in items)
        {
            double? cost = 0;
            cost = item.retailPrice * HoursInMonth * skuData?.capacity;

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

        if(skuData != null && skuData.name != null)
        {
            var skuName = skuData.name.Split("_")[1];
            var isFlash = skuName.StartsWith("F");
            var divider = isFlash ? 3 : 2;
            var licenseCost = LicenseCost[skuName] * (skuData?.capacity / divider);

            summary.DetailedCost.Add("License Cost", licenseCost);
            estimatedCost += licenseCost;
        }   
        
        summary.TotalCost = estimatedCost.GetValueOrDefault();
        return summary;
    }
}
