using ACE.Calculation;
using ACE.WhatIf;

internal class BotServiceEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public BotServiceEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change, double conversionRate)
        : base(items, id, change, conversionRate)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public TotalCostSummary GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        return new TotalCostSummary();
    }
}
