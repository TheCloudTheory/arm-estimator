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

    public double GetTotalCost(WhatIfChange[] changess, IDictionary<string, string>? usagePatterns)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        
        foreach(var item in items)
        {
            if (item.meterName == "Basic Registry Unit")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterName == "Standard Registry Unit")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterName == "Premium Registry Unit")
            {
                estimatedCost += item.retailPrice * 30;
            }
            else if (item.meterName == "Data Stored")
            {
                estimatedCost += item.retailPrice * base.IncludeUsagePattern("Microsoft_ContainerRegistry_registries_Data_Stored", usagePatterns);
            }
            else if (item.meterName == "Task vCPU Duration")
            {
                estimatedCost += item.retailPrice * base.IncludeUsagePattern("Microsoft_ContainerRegistry_registries_Task_vCPU_Duration", usagePatterns);
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
