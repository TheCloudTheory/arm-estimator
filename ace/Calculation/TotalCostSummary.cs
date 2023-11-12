namespace ACE.Calculation;

internal class TotalCostSummary
{
    public double TotalCost { get; set; }
    public IDictionary<string, double?> DetailedCost { get; }

    public TotalCostSummary()
    {
        TotalCost = 0;
        DetailedCost = new Dictionary<string, double?>();
    }
}