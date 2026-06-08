using System.Text.Json.Serialization;

internal class AKSProperties
{
    [JsonPropertyName("agentPoolProfiles")]
    public AgentPoolProfile[]? AgentPoolProfiles_ARM { get; set; }

    [JsonPropertyName("default_node_pool")]
    public AgentPoolProfile_TF[]? AgentPoolProfiles_TF { get; set; }

    public AgentPoolProfile[]? AgentPoolProfiles => AgentPoolProfiles_ARM == null ? AgentPoolProfiles_TF : AgentPoolProfiles_ARM;
}

internal class AgentPoolProfile
{
    [JsonPropertyName("count")]
    public virtual int Count { get; set; }

    [JsonPropertyName("osType")]
    public virtual string? OsType { get; set; }

    [JsonPropertyName("type")]
    public virtual string? Type { get; set; }

    [JsonPropertyName("vmSize")]
    public virtual string? VmSize { get; set; }

    [JsonPropertyName("enableUltraSSD")]
    public virtual bool? EnableUltraSSD { get; set; }

    [JsonPropertyName("osDiskSizeGB")]
    public virtual int? OSDiskSizeGB { get; set; }
}

internal class AgentPoolProfile_TF : AgentPoolProfile
{
    [JsonPropertyName("node_count")]
    public override int Count { get; set; }

    [JsonPropertyName("os_sku")]
    public override string? OsType { get; set; } = "Linux";

    [JsonPropertyName("type")]
    public override string? Type { get; set; }

    [JsonPropertyName("vm_size")]
    public override string? VmSize { get; set; }

    [JsonPropertyName("ultra_ssd_enabled")]
    public override bool? EnableUltraSSD { get; set; }

    [JsonPropertyName("os_disk_size_gb")]
    public override int? OSDiskSizeGB { get; set; }
}