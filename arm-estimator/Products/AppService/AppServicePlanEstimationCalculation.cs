internal class AppServicePlanEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AppServicePlanEstimationCalculation(RetailItem[] items)
        : base(items)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        var consumptionMetrics = this.items.Where(_ => _.type != "Reservation");

        return this.items.Where(_ => _.type != "Reservation").OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost()
    {
        var items = GetItems();
        var estimatedCost = items.Select(_ => _.retailPrice).Sum();

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
