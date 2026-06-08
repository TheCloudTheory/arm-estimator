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

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("properties")]
    public PeeringProperties? Properties { get; set; }
}

internal class PeeringProperties
{
    [JsonPropertyName("remoteVirtualNetwork")]
    public RemoteVirtualNetwork? RemoteVirtualNetwork { get; set; }
}

internal class RemoteVirtualNetwork
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}