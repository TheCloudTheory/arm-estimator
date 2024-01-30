using System.Text.Json.Serialization;

namespace ACE.Products.ASR;

internal class ReplicationFabricProperties
{
    [JsonPropertyName("customDetails")]
    public CustomDetails CustomDetails { get; set; } = new CustomDetails();
}

internal class CustomDetails
{
    [JsonPropertyName("instanceType")]
    public string InstanceType { get; set; } = null!;

    [JsonPropertyName("location")]

    public string Location { get; set; } = null!;
}
