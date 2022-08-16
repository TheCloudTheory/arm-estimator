using Azure.Core;

internal class VPNGatewayEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public VPNGatewayEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changes)
    {
        double? estimatedCost = 0;
        var items = GetItems();

        foreach (var item in items)
        {
            if (item.meterName == "Basic Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Standard Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "Ultra High Performance Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "High Performance gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw1")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw3")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw4")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGw5")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ1")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ3")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ4")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "VpnGwAZ5")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "ErGw1AZ Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "ErGw2AZ Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "ErGw3AZ Gateway")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
