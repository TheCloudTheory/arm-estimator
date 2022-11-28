using System.Text.Json.Serialization;

internal class TemplateSchema
{
    [JsonPropertyName("parameters")]
    public IDictionary<string, TemplateParameter>? Parameters { get; set; }

    [JsonPropertyName("metadata")]
    public MetadataSchema? Metadata { get; set; }
}

internal class MetadataSchema
{
    [JsonPropertyName("aceUsagePatterns")]
    public IDictionary<string, string>? UsagePatterns { get; set; }
}

internal class TemplateParameter
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}