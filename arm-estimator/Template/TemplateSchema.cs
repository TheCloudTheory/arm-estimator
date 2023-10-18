using System.Text.Json.Serialization;

internal class TemplateSchema
{
    [JsonPropertyName("parameters")]
    public IDictionary<string, TemplateParameter>? Parameters { get; set; }

    [JsonPropertyName("metadata")]
    public MetadataSchema? Metadata { get; set; }

    [JsonPropertyName("resources")]
    public SpecialCaseResourceSchema[]? SpecialCaseResources { get; set; }
}

internal class MetadataSchema
{
    [JsonPropertyName("aceUsagePatterns")]
    public IDictionary<string, string>? UsagePatterns { get; set; }
}

internal class TemplateParameter
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    public object? Value { get; set; }
}

internal class SpecialCaseResourceSchema
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("properties")]
    public SpecialCaseResourcePropertiesSchema Properties { get; set; } = null!;
}

internal class SpecialCaseResourcePropertiesSchema
{
    [JsonPropertyName("virtualNetworkPeerings")]
    public VirtualNetworkPeeringSchema[]? VirtualNetworkPeerings { get; set; }
}

internal class VirtualNetworkPeeringSchema
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
}