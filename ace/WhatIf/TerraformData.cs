using System.Text.Json.Serialization;

namespace ACE.WhatIf;

internal class TerraformData
{
    [JsonPropertyName("resource_changes")]
    public ResourceChange[]? Changes { get; set; }

    [JsonPropertyName("configuration")]
    public TerraformConfiguration? Configuration { get; set; }
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
    [JsonPropertyName("actions")]
    public string[]? Actions { get; set; }

    [JsonPropertyName("before")]
    public IDictionary<string, object>? Before { get; set; }

    [JsonPropertyName("after")]
    public IDictionary<string, object>? After { get; set; }

    [JsonPropertyName("after_unknown")]
    public IDictionary<string, object>? AfterUnknown { get; set; }
}

internal class TerraformConfiguration
{
    [JsonPropertyName("root_module")]
    public RootModule? RootModule { get; set; }
}

internal class RootModule
{
    [JsonPropertyName("resources")]
    public ModuleResource[]? Resources { get; set; }
}

internal class ModuleResource
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("expressions")]
    public Expression? Expressions { get; set; }
}

internal class Expression
{
    [JsonPropertyName("resource_group_name")]
    public ResourceGroupReferences? ResourceGroup { get; set; }
}

internal class ResourceGroupReferences
{
    [JsonPropertyName("references")]
    public string[]? References { get; set; }
}