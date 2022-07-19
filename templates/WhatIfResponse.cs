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
}