using Azure.Core;

internal class EstimationOutput
{
    public double TotalCost { get; }
    public double Delta { get; }
    public IEnumerable<EstimatedResourceData> Resources { get; }
    public string Currency { get; }

    public EstimationOutput(double totalCost, double delta, IEnumerable<EstimatedResourceData> resources, CurrencyCode currency)
    {
        TotalCost = totalCost;
        Delta = delta;
        Resources = resources;
        Currency = currency.ToString();
    }
}

internal class EstimatedResourceData
{
    public string Id { get; }
    public double TotalCost { get; }
    public double Delta { get; }

    public EstimatedResourceData(double totalCost, double? delta, ResourceIdentifier id)
    {
        TotalCost = totalCost;
        Delta = delta == null ? totalCost : 0;
        Id = id.ToString();
    }
}