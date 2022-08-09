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
            // Basic Gateway
            if (item.meterId == "c90286c8-adf0-438e-a257-4468387df385")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // Standard gateway
            else if (item.meterId == "ec0d8d1f-d3ba-48aa-be63-02812d369dea")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // Standard gateway
            else if (item.meterId == "ec0d8d1f-d3ba-48aa-be63-02812d369dea")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // Ultra High Performance Gateway
            else if (item.meterId == "bc234a3d-532e-4117-b6f6-a16dc199cfd6")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // High Performance gateway
            else if (item.meterId == "2300a154-e1df-4476-9f87-c36378745baa")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGw1
            else if (item.meterId == "431b835b-2538-4faa-8763-8202f3fdbbb4")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGw2
            else if (item.meterId == "907aacb3-4b68-4c57-9af2-3cf4505285b1")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGw3
            else if (item.meterId == "5dc9b160-8b1f-4a9c-83ed-7e458511912d")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGw4
            else if (item.meterId == "504a49d3-d266-43ff-81b6-3a0c238e285d")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGw5
            else if (item.meterId == "609f8f46-51df-42ec-a7d9-cd3a49ce7e79")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGwAZ1
            else if (item.meterId == "f7f36e66-5b21-445a-9df1-f192e56ed76c")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGwAZ2
            else if (item.meterId == "685a81da-cbd5-4c39-9b90-4ca776cd5a0d")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGwAZ3
            else if (item.meterId == "83bb6f61-37a3-4741-a235-21142aeaaa84")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGwAZ4
            else if (item.meterId == "cb9ac5ee-c290-5e76-8928-1ce5253f7a8f")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // VpnGwAZ5
            else if (item.meterId == "90086093-9aac-537a-9f05-3ab9aaea2c1d")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // ErGw1AZ Gateway
            else if (item.meterId == "7821a897-fb13-4847-9357-e7447fb94b17")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // ErGw2AZ Gateway
            else if (item.meterId == "74ee2502-1613-407d-a437-f445625d1172")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // ErGw3AZ Gateway
            else if (item.meterId == "b4a5b140-2d64-4f7f-93fe-f1437d5d8cbe")
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
