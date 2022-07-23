using System.Text.Json.Serialization;

internal class WhatIfResponse
{
    public string? status { get; set; }
    public WhatIfError? error { get; set; }
    public WhatIfProperties? properties { get; set; }
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

internal class WhatIfChange
{
    public string? resourceId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WhatIfChangeType? changeType { get; set; }
    public WhatIfAfterChange? after { get; set; }
}

internal class WhatIfAfterChange
{
    public string? location { get; set; }
    public WhatIfSku? sku { get; set; }
    public string? kind { get; set; }
    public IDictionary<string, object>? properties { get; set; }

}

internal class WhatIfSku
{
    public string? name { get; set; }
    public string? tier { get; set; }
}