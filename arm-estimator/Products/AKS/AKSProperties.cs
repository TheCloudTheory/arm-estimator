using System.Text.Json.Serialization;

internal class AKSProperties
{
    [JsonPropertyName("agentPoolProfiles")]
    public AgentPoolProfile[]? AgentPoolProfiles { get; set; }
}

internal class AgentPoolProfile
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("osType")]
    public string? OsType { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("vmSize")]
    public string? VmSize { get; set; }
}