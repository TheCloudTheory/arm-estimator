using ACE.WhatIf;

internal class ChangeMessage
{
    public ChangeMessage(WhatIfChangeType changeType, string message)
    {
        ChangeType = changeType;
        Message = message;
    }

    public WhatIfChangeType ChangeType { get; }
    public string Message { get; }
}
