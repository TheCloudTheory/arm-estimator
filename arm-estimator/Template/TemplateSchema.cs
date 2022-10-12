using System.Text.Json.Serialization;

internal class TemplateSchema
{
    [JsonPropertyName("parameters")]
    public IDictionary<string, TemplateParameter>? Parameters { get; set; }
}

internal class TemplateParameter
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}