using ACE.Calculation;
using ACE.WhatIf;

internal class KeyVaultEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public KeyVaultEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change, double conversionRate)
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

            if (item.meterName == "Standard B1 Instance")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if(item.meterName == "Certificate Renewal Requests")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_KeyVault_vaults_Certificate_Renewal_Requests", usagePatterns, 1);
            }
            else if(item.meterName == "Automated Key Rotation")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_KeyVault_vaults_Automated_Key_Rotation", usagePatterns, 0);
            }
            else if(item.meterName == "Secret Renewal")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_KeyVault_vaults_Secret_Renewal", usagePatterns, 0);
            }
            else if(item.meterName == "Advanced Key Operations")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_KeyVault_vaults_Advanced_Key_Operations", usagePatterns, 10000) / 10000;
            }
            else if(item.meterName == "Operations")
            {
                cost = item.retailPrice * base.IncludeUsagePattern("Microsoft_KeyVault_vaults_Operations", usagePatterns, 10000) / 10000;
            }
            else
            {
                cost = item.retailPrice ;
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
