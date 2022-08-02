using Azure.Core;

internal class ContainerAppsEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public ContainerAppsEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
        : base(items, id, change)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        var consumptionMetrics = this.items.Where(_ => _.type != "Reservation");

        return this.items.Where(_ => _.type != "Reservation").OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost(WhatIfChange[] changes)
    {
        double? estimatedCost = 0;
        var items = GetItems();
        
        foreach(var item in items)
        {
            // vCPU active usage
            if(item.meterId == "0426badc-b719-5201-b141-b2d5c4943b7d")
            {
                estimatedCost += item.retailPrice * HoursInMonth / 2 * 3600;
            }
            // Memory active usage
            else if (item.meterId == "3ba64b1c-cb46-5ee0-b086-a68736392ac0")
            {
                estimatedCost += item.retailPrice * HoursInMonth / 2 * 3600;
            }
            // vCPU idle usage
            else if (item.meterId == "7b7747f8-2c0b-5b6b-a6d9-d998e92f6bcc")
            {
                estimatedCost += item.retailPrice * HoursInMonth / 2 * 3600;
            }
            // Memory idle usage
            else if (item.meterId == "cea8b5c0-c6fe-59cf-baa7-123dbc4c6bb8")
            {
                estimatedCost += item.retailPrice * HoursInMonth / 2 * 3600;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
