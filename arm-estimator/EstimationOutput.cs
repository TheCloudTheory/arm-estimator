using ACE.WhatIf;

namespace ACE;

public class EstimationOutput
{
    public double TotalCost { get; }
    public double Delta { get; }
    public IEnumerable<EstimatedResourceData> Resources { get; }
    public string Currency { get; }
    public int TotalResourceCount { get; }
    public int EstimatedResourceCount { get; }
    public int SkippedResourceCount { get; }

    internal EstimationOutput(double totalCost,
                              double delta,
                              IEnumerable<EstimatedResourceData> resources,
                              CurrencyCode currency,
                              int totalResourcesCount,
                              int skippedResourcesCount)
    {
        TotalCost = totalCost;
        Delta = delta;
        Resources = resources;
        Currency = currency.ToString();
        TotalResourceCount = totalResourcesCount;
        EstimatedResourceCount = resources.Count();
        SkippedResourceCount = skippedResourcesCount;
    }
}

public class EstimatedResourceData
{
    public string Id { get; }
    public double TotalCost { get; }
    public double Delta { get; }

    public EstimatedResourceData(double totalCost, double? delta, CommonResourceIdentifier id)
    {
        TotalCost = totalCost;
        Delta = delta == null ? totalCost : 0;
        Id = id.ToString();
    }
}