using Azure.Core;

internal class VNetEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public VNetEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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

            if (item.meterName == "Inbound data transfer")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_Network_virtualNetworks_Inbound_data_transfer", usagePatterns);
            }
            else if (item.meterName == "Outbound data transfer")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_Network_virtualNetworks_Outbound_data_transfer", usagePatterns);
            }
            else
            {
                cost += item.retailPrice;
            }      

            estimatedCost += cost;

            if(summary.DetailedCost.ContainsKey(item.meterName!))
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
