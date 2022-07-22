internal class AppServicePlanEstimationCalculation : BaseEstimation, IEstimationCalculation
{
    public AppServicePlanEstimationCalculation(RetailItem[] items)
        : base(items)
    {
    }

    public IOrderedEnumerable<RetailItem> GetItems()
    {
        return this.items.Where(_ => _.type != "Reservation" && _.type != "DevTestConsumption")
            .OrderByDescending(_ => _.retailPrice);
    }

    // TODO: Functions Premium plan calculation should include multiplication if used EP2 / EP3
    public double GetTotalCost()
    {
        double? estimatedCost = 0;
        var items = GetItems();

        foreach(var item in items)
        {
            // vCPU Duration
            if (item.meterId == "2099ccfe-9c25-4ae2-9e35-6500db3b8e74")
            {
                estimatedCost += item.retailPrice * 720;
            }
            // Memory Duration
            else if (item.meterId == "8fca9dc0-1307-4b8c-986c-d58505cf1e4b")
            {
                estimatedCost += item.retailPrice * 720;
            }
            else
            {
                estimatedCost += item.retailPrice;
            }
        }

        var premiumFunctionsCpuMetric = items.SingleOrDefault(_ => _.meterId == "2099ccfe-9c25-4ae2-9e35-6500db3b8e74");
        var premiumFunctionsMemory = items.SingleOrDefault(_ => _.meterId == "8fca9dc0-1307-4b8c-986c-d58505cf1e4b");

        return estimatedCost == null ? 0 : (double)estimatedCost;
    }
}
