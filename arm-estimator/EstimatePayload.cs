using System.Text.Json.Serialization;

internal class EstimatePayload
{
    public EstimatePayloadProperties properties {get;}

    public EstimatePayload(string template)
    {
        properties = new EstimatePayloadProperties(template);
    }
}

internal class EstimatePayloadProperties
{
    [JsonConverter(typeof(RawJsonConverter))]
    public string template {get;}
    public object parameters = new object();

    public EstimatePayloadProperties(string template)
    {
        this.template = template;
    }

    public string mode => "Incremental";
}