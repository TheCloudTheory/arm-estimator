internal class ContainerRegistryEstimationCalculation : IEstimationCalculation
{
    private readonly RetailItem[] items;

    public ContainerRegistryEstimationCalculation(RetailItem[] items)
    {
        this.items = items;
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        var consumptionMetrics = this.items.Where(_ => _.type != "Reservation");

        return this.items.Where(_ => _.type != "Reservation").OrderByDescending(_ => _.retailPrice);
    }

    public double GetTotalCost()
    {
        double? estimatedCost;
        var items = GetItems();

        // If metrics contain "Basic Registry Unit" metric, we need to calculate a full
        // month estimation as they are presented as single-day metric
        var basicRegistryUnitMetric = items.SingleOrDefault(_ => _.meterId == "5c9e7a65-5784-494c-9718-7749d4075dd9");
        if (basicRegistryUnitMetric != null)
        {
            estimatedCost = items.Where(_ => _.meterId != "5c9e7a65-5784-494c-9718-7749d4075dd9").Select(_ => _.retailPrice).Sum();
            estimatedCost += basicRegistryUnitMetric.retailPrice * 30;
        }
        else
        {
            estimatedCost = items.Select(_ => _.retailPrice).Sum();
        }

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
