using System.Text.Json.Serialization;

namespace ACE.Template;

internal class BicepparamParametersSchema
{
    [JsonPropertyName("parametersJson")]
    public string? ParametersJson { get; set; }
}
