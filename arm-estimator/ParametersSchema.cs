using System.Text.Json.Serialization;

internal class ParametersSchema
{
    [JsonPropertyName("$schema")]
    public string? Schema { get; set; }

    [JsonPropertyName("contentVersion")]
    public string? ContentVersion { get; set; }

    [JsonPropertyName("parameters")]
    public IDictionary<string, Parameter>? Parameters { get; set; }

    public ParametersSchema()
    {
        Schema = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#";
        ContentVersion = "1.0.0.0";
    }
}

internal class Parameter
{
    [JsonPropertyName("value")]
    public object? Value { get; set; }
}