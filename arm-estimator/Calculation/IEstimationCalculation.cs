internal interface IEstimationCalculation
{
    double GetTotalCost();
    IOrderedEnumerable<RetailItem> GetItems();
}
