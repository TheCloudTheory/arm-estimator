using ACE.WhatIf;

namespace ACE.Output;

/// <summary>Aggregate result returned by <see cref="ACE.Core.EstimationService.EstimateAsync"/>.</summary>
public class EstimationOutput
{
    public HumanFriendlyCost TotalCost { get; }
    public HumanFriendlyCost Delta { get; }
    public IEnumerable<EstimatedResourceData> Resources { get; }
    public string Currency { get; }
    public int TotalResourceCount { get; }
    public int EstimatedResourceCount { get; }
    public int SkippedResourceCount { get; }

    internal EstimationOutput( // constructed only by WhatIfProcessor
                              double totalCost,
                              double delta,
                              IEnumerable<EstimatedResourceData> resources,
                              CurrencyCode currency,
                              int totalResourcesCount,
                              int skippedResourcesCount)
    {
        TotalCost = new HumanFriendlyCost(totalCost);
        Delta = new HumanFriendlyCost(delta);
        Resources = resources;
        Currency = currency.ToString();
        TotalResourceCount = totalResourcesCount;
        EstimatedResourceCount = resources.Count();
        SkippedResourceCount = skippedResourcesCount;
    }
}

/// <summary>Per-resource cost breakdown entry within an <see cref="EstimationOutput"/>.</summary>
public class EstimatedResourceData
{
    public string Id { get; }
    public HumanFriendlyCost TotalCost { get; }
    public HumanFriendlyCost Delta { get; }

    public EstimatedResourceData(double totalCost, double? delta, CommonResourceIdentifier id)
    {
        TotalCost = new HumanFriendlyCost(totalCost);
        Delta = delta == null ? new HumanFriendlyCost(totalCost) : new HumanFriendlyCost(0);
        Id = id.ToString();
    }
}