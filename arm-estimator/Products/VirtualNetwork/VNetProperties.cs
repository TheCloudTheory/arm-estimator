using System.Text.Json.Serialization;

internal class VNetProperties
{
    [JsonPropertyName("virtualNetworkPeerings")]
    public Peering[]? Peerings { get; set; }
}

internal class Peering
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}