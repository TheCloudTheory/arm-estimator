using System.Text.Json.Serialization;

namespace ACE.Compilation;

internal class BicepparamStdoutResult
{
    [JsonPropertyName("parametersJson")]
    public string ParametersJson { get; set; } = null!;
}