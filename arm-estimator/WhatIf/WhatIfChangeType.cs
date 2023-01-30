namespace ACE.WhatIf;

internal enum WhatIfChangeType
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