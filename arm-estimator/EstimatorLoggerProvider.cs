using Microsoft.Extensions.Logging;

internal class EstimatorLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new EstimatorLogger();
    }

    public void Dispose()
    {
    }
}
