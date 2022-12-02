using Azure.Core;

internal class ContainerRegistryEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public ContainerRegistryEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public TotalCostSummary GetTotalCost(WhatIfChange[] changess, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        var summary = new TotalCostSummary();
        
        foreach(var item in items)
        {
            double? cost = 0;

            if (item.meterName == "Basic Registry Unit" || item.meterName == "Standard Registry Unit" || item.meterName == "Premium Registry Unit")
            {
                cost = item.retailPrice * 30;
            }
            else if (item.meterName == "Data Stored")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_ContainerRegistry_registries_Data_Stored", usagePatterns);
            }
            else if (item.meterName == "Task vCPU Duration")
            {
                cost += item.retailPrice * base.IncludeUsagePattern("Microsoft_ContainerRegistry_registries_Task_vCPU_Duration", usagePatterns);
            }
            else
            {
                cost += item.retailPrice;
            }

            estimatedCost += cost;
            summary.DetailedCost.Add(item.meterName!, cost);
        }

        summary.TotalCost = estimatedCost.GetValueOrDefault();

        return summary;
    }
}
