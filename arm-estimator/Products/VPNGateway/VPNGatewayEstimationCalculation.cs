using ACE.WhatIf;
using Azure.Core;

internal class VPNGatewayEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public VPNGatewayEstimationCalculation(RetailItem[] items, CommonResourceIdentifier id, WhatIfAfterBeforeChange change)
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

            if (item.meterName == "Basic Gateway")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Standard Gateway")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Ultra High Performance Gateway")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "High Performance gateway")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw1")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw2")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw3")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw4")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw5")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ1")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ2")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ3")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ4")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ5")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "ErGw1AZ Gateway")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "ErGw2AZ Gateway")
            {
                cost = item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "ErGw3AZ Gateway")
            {
                cost = item.retailPrice * HoursInMonth;
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
}
