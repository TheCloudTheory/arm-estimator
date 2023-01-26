using ACE.Calculation;
using ACE.WhatIf;

internal class DiagnosticSettingsEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    private readonly LogAnalyticsEstimationCalculation laCalculation;
    private readonly StorageAccountEstimationCalculation storageCalculation;
    private readonly EventHubEstimationCalculation ehCalculation;

    public DiagnosticSettingsEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
        var laItems = items.Where(_ => _.serviceId == LogAnalyticsQueryFilter.ServiceId).ToArray();
        var storageItems = items.Where(_ => _.serviceId == StorageAccountQueryFilter.ServiceId).ToArray();
        var ehItems = items.Where(_ => _.serviceId == EventHubQueryFilter.ServiceId).ToArray();

        this.laCalculation = new LogAnalyticsEstimationCalculation(laItems, id, change);
        this.storageCalculation = new StorageAccountEstimationCalculation(storageItems, id, change);
        this.ehCalculation = new EventHubEstimationCalculation(ehItems, id, change);
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public TotalCostSummary GetTotalCost(WhatIfChange[] changes, IDictionary<string, string>? usagePatterns)
    {
        var laSummary = this.laCalculation.GetTotalCost(changes, usagePatterns);
        var storageSummary = this.storageCalculation.GetTotalCost(changes, usagePatterns);
        var ehSummary = this.ehCalculation.GetTotalCost(changes, usagePatterns);

        var summary = new TotalCostSummary();
        summary.TotalCost = laSummary.TotalCost + storageSummary.TotalCost + ehSummary.TotalCost;
        
        foreach(var item in laSummary.DetailedCost)
        {
            summary.DetailedCost.Add(item);
        }

        foreach (var item in storageSummary.DetailedCost)
        {
            summary.DetailedCost.Add(item);
        }

        foreach (var item in ehSummary.DetailedCost)
        {
            summary.DetailedCost.Add(item);
        }

        return summary;
    }
}
