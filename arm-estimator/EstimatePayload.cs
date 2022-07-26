using System.Text.Json.Serialization;

internal class EstimatePayload
{
    public EstimatePayloadProperties properties {get;}

    public EstimatePayload(string template, DeploymentMode deploymentMode)
    {
        properties = new EstimatePayloadProperties(template, deploymentMode);
    }
}

internal class EstimatePayloadProperties
{
    [JsonConverter(typeof(RawJsonConverter))]
    public string template { get; }
    public object parameters = new object();

    public EstimatePayloadProperties(string template, DeploymentMode deploymentMode)
    {
        this.template = template;
        this.mode = deploymentMode.ToString();
    }

    public string mode { get; }
}