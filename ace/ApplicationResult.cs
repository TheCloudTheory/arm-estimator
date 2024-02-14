namespace ACE;

internal sealed class ApplicationResult(int exitCode, string? errorMessage)
{
    public int ExitCode { get; } = exitCode;
    public string? ErrorMessage { get; } = errorMessage;
}