using Microsoft.Extensions.Logging;

internal class EstimatorLogger : ILogger
{
    public IDisposable BeginScope<TState>(TState state) => default!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel) == false) return;

        if(exception != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.ToString());
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine(formatter(state, exception));
        }
    }
}