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
            if (item.meterName == "vCPU Duration" && item.productName != "Logic Apps")
            {
                estimatedCost += item.retailPrice * HoursInMonth * capacity;
            }
            else if (item.meterName == "Memory Duration" && item.productName != "Logic Apps")
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
            else if (item.meterName == "vCPU Duration" && item.productName == "Logic Apps")
            {
                estimatedCost += item.retailPrice * HoursInMonth * vCpuCapacity;
            }
            else if (item.meterName == "Memory Duration" && item.productName == "Logic Apps")
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
