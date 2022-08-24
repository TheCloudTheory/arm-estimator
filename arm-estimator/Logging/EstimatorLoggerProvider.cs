using Microsoft.Extensions.Logging;

internal class EstimatorLoggerProvider : ILoggerProvider
{
    private readonly bool isSilent;

    public EstimatorLoggerProvider(bool isSilent)
    {
        this.isSilent = isSilent;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new EstimatorLogger(this.isSilent);
    }

    public void Dispose()
    {
    }
}
