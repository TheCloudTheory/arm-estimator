﻿using Azure.Core;

internal class TimeSeriesEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public TimeSeriesEstimationCalculation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
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
        var capacity = this.change.sku?.capacity;

        if (capacity == null)
        {
            capacity = 1;
        }

        foreach (var item in items)
        {
            estimatedCost += item.retailPrice * 30 * capacity;
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}