internal interface IEstimationCalculation
{
    double GetTotalCost(WhatIfChange[] changes);
    IOrderedEnumerable<RetailItem> GetItems();
}
