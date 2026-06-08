using ACE.Compilation;
using System.Text.Json.Serialization;

namespace ACE.WhatIf;

internal class WhatIfResponse
{
    public string? status { get; set; }
    public WhatIfError? error { get; set; }
    public WhatIfProperties? properties { get; set; }
    public TemplateType? TemplateType { get; set; }
}

internal class WhatIfError
{
    public string? code { get; set; }
    public string? message { get; set; }
    public WhatIfErrorDetail[]? details { get; set; }
}

internal class WhatIfErrorDetail
{
    public string? code { get; set; }
    public string? message { get; set; }
    public WhatIfSpecificErrorDetail[]? details { get; set; }
}

internal class WhatIfSpecificErrorDetail
{
    public string? code { get; set; }
    public string? target { get; set; }
    public string? message { get; set; }
}

internal class WhatIfProperties
{
    public WhatIfChange[]? changes { get; set; }
}

/// <summary>Describes a single resource change in a What-If operation or inventory snapshot.</summary>
public class WhatIfChange
{
    public string? resourceId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WhatIfChangeType? changeType { get; set; }
    public WhatIfAfterBeforeChange? after { get; set; }
    public WhatIfAfterBeforeChange? before { get; set; }
}

/// <summary>ARM resource state snapshot (before or after a change).</summary>
public class WhatIfAfterBeforeChange
{
    public string? location { get; set; }
    public WhatIfSku? sku { get; set; }
    public string? kind { get; set; }
    public IDictionary<string, object?>? properties { get; set; }
    public string? type { get; set; }
    public OmsPlan? plan { get; set; }
}

/// <summary>OMS/Marketplace plan attached to a resource.</summary>
public class OmsPlan
{
    public string? product { get; set; }
}

/// <summary>SKU information for a resource.</summary>
public class WhatIfSku
{
    public string? name { get; set; }
    public string? tier { get; set; }
    public int? capacity { get; set; }
    public string? size { get; set; }
}