using Microsoft.Extensions.Logging;

internal class EstimatorLoggerProvider : ILoggerProvider
{
    private readonly bool isSilent;
    private readonly string? logFile;

    public EstimatorLoggerProvider(bool isSilent, string? logFile)
    {
        this.isSilent = isSilent;
        this.logFile = logFile;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new EstimatorLogger(this.isSilent, this.logFile);
    }

    public void Dispose()
    {
    }
}
