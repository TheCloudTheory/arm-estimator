using System.Text.Json.Serialization;

namespace ACE.Products.ASR;

internal class ReplicationProtectedItemProperties
{
    [JsonPropertyName("providerSpecificDetails")]
    public ProviderSpecificDetails ProviderSpecificDetails { get; set; } = new ProviderSpecificDetails();
}

internal class ProviderSpecificDetails
{
    [JsonPropertyName("fabricObjectId")]
    public string FabricObjectId { get; set; } = null!;

    [JsonPropertyName("instanceType")]
    public string InstanceType { get; set; } = null!;
}