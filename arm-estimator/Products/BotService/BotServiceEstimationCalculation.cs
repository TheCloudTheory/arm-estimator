using ACE.Calculation;
using ACE.WhatIf;
using Azure.Core;

internal class BotServiceEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public BotServiceEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
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
