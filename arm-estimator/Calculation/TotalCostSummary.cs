internal class TotalCostSummary
{
    public double TotalCost { get; set; }
    public IDictionary<string, double?> DetailedCost => new Dictionary<string, double?>();

    public TotalCostSummary()
    {
        TotalCost = 0;
    }
}