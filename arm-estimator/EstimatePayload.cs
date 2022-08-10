using System.Text.Json;
using System.Text.Json.Serialization;

internal class EstimatePayload
{
    public EstimatePayloadProperties properties { get; }

    public EstimatePayload(string template, DeploymentMode deploymentMode, string parameters)
    {
        properties = new EstimatePayloadProperties(template, deploymentMode, parameters);
    }
}

internal class EstimatePayloadProperties
{
    [JsonConverter(typeof(RawJsonConverter))]
    public string template { get; }

    [JsonConverter(typeof(RawJsonConverter))]
    public string parameters { get; }

    public EstimatePayloadProperties(string template, DeploymentMode deploymentMode, string parameters)
    {
        var templateParametersObject = JsonSerializer.Deserialize<TemplateParameters>(parameters);

        this.template = template;
        this.parameters = JsonSerializer.Serialize(templateParametersObject?.parameters);
        this.mode = deploymentMode.ToString();
    }

    public string mode { get; }
}

internal class TemplateParameters
{
    [JsonPropertyName("$schema")]
    public string? schema { get; set; }

    public string? contentVersion { get; set; }
    public Dictionary<string, object>? parameters { get; set; }
}