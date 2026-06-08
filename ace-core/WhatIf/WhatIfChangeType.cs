namespace ACE.WhatIf;

/// <summary>The type of change detected or declared for a resource.</summary>
public enum WhatIfChangeType
{
    Create,
    Delete,
    Deploy,
    Ignore,
    Modify,
    NoChange,
    Unsupported,
    Unknown
}