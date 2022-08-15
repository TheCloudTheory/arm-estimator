using Azure.Core;

internal class AppServicePlanEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AppServicePlanEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var sku = this.change.sku?.name;
        var capacity = 1;
        var vCpuCapacity = 1;
        var memoryCapacity = 3.5;

        if(sku != null)
        {
            if (IsSkuOfLogicApp(sku))
            {
                if (sku.EndsWith("2"))
                {
                    vCpuCapacity = 2;
                    memoryCapacity = 7;
                }

                if (sku.EndsWith("3"))
                {
                    vCpuCapacity = 4;
                    memoryCapacity = 14;
                }
            }
            else
            {
                if (sku.EndsWith("2"))
                {
                    capacity = 2;
                }

                if (sku.EndsWith("3"))
                {
                    capacity = 4;
                }
            }
        }

        foreach (var item in items)
        {
            // vCPU Duration (Functions)
            if (item.meterName == "2099ccfe-9c25-4ae2-9e35-6500db3b8e74")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            // Memory Duration (Functions)
            else if (item.meterName == "8fca9dc0-1307-4b8c-986c-d58505cf1e4b")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Shared")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "B1")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "B2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "B3")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "S1")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "S2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "S3")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "P1")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "P2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "P3")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "P1 v2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "P2 v2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "P3 v2")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "P1 v3")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "P2 v3")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            else if (item.meterName == "P3 v3")
            {
                estimatedCost += item.retailPrice * HoursInMonth;
            }
            // vCPU Duration (LogicApps)
            if (item.meterName == "031d9d87-a8d3-546a-8a57-9afe80dbb478")
            {
                estimatedCost += item.retailPrice * HoursInMonth * vCpuCapacity;
            }
            // Memory Duration  (LogicApps)
            else if (item.meterName == "7824092a-b733-5933-9b38-c06ff544977f")
            {
                estimatedCost += item.retailPrice * HoursInMonth * memoryCapacity;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }

    private static bool IsSkuOfLogicApp(string sku)
    {
        return sku.StartsWith("WS");
    }
}
