using System.Text.Json.Serialization;

namespace ACE.WhatIf;

internal class TerraformData
{
    [JsonPropertyName("resource_changes")]
    public ResourceChange[]? Changes { get; set; }
}

internal class ResourceChange
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("change")]
    public ResourceChangeData? Change { get; set; }
}

internal class ResourceChangeData
{
    [JsonPropertyName("before")]
    public IDictionary<string, object>? Before { get; set; }

    [JsonPropertyName("after")]
    public IDictionary<string, object>? After { get; set; }
}
