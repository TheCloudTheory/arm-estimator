using ACE.WhatIf;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ACE;

internal class EstimatePayload
{
    public EstimatePayloadProperties properties { get; }
    public string? location { get; }

    public EstimatePayload(string template, DeploymentMode deploymentMode, string parameters)
    {
        properties = new EstimatePayloadProperties(template, deploymentMode, parameters);
    }

    public EstimatePayload(string template, DeploymentMode deploymentMode, string parameters, string? location)
    {
        properties = new EstimatePayloadProperties(template, deploymentMode, parameters);
        this.location = location;
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
        this.parameters = JsonSerializer.Serialize(templateParametersObject?.parameters, new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        mode = deploymentMode.ToString();
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