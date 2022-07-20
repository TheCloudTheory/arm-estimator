internal class WhatIfResponse
{
    public WhatIfProperties? properties { get; set; }
}

internal class WhatIfProperties
{
    public WhatIfChange[]? changes { get; set; }
}

internal class WhatIfChange
{
    public string? resourceId { get; set; }
    public string? changeType { get; set; }
    public WhatIfAfterChange? after { get; set; }
}

internal class WhatIfAfterChange
{
    public string? location { get; set; }
    public WhatIfSku? sku { get; set; }
}

internal class WhatIfSku
{
    public string? name { get; set; }
}