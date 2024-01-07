using ACE;
using System.Text.Json.Serialization;

namespace ACE_Tests;
internal class EstimationOutput
{
    public HumanFriendlyCost TotalCost { get; set; }
    public HumanFriendlyCost Delta { get; set; }
    public IEnumerable<EstimatedResourceData> Resources { get; set; }
    public string Currency { get; set; }
    public int TotalResourceCount { get; set; }
    public int EstimatedResourceCount { get; set; }
    public int SkippedResourceCount { get; set; }

    [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public EstimationOutput() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}

internal class EstimatedResourceData
{
    public string Id { get; set; }
    public HumanFriendlyCost TotalCost { get; set; }
    public HumanFriendlyCost Delta { get; set; }

    [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public EstimatedResourceData() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
