using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class EstimatorLogger : ILogger
{
    private readonly bool isSilent;
    private readonly string? logFile;

    IDisposable ILogger.BeginScope<TState>(TState state) => default!;
    public bool IsEnabled(LogLevel logLevel) => true;

    public EstimatorLogger(bool isSilent, string? logFile)
    {
        this.isSilent = isSilent;
        this.logFile = logFile;

        ClearLogFile();
    }

    private void ClearLogFile()
    {
        if (this.logFile != null)
        {
            File.WriteAllText($"{this.logFile}.log", string.Empty);
        }
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel) == false) return;

        if(exception != null)
        {
            Console.WriteLine(exception);
            WriteToLogFile(exception.ToString());
        }
        else
        {
            if(typeof(TState) == typeof(NonSilentMessage))
            {
                Console.WriteLine(formatter(state, exception));
                WriteToLogFile(formatter(state, exception));
                return;
            }

            if (this.isSilent) return;

            if(typeof(TState) == typeof(ChangeMessage))
            {
                if (state is ChangeMessage message)
                {
                    Console.Write("[");
                    Console.ForegroundColor = GetConsoleColorBasedOnChange(message.ChangeType);
                    Console.Write(message.ChangeType);
                    Console.ResetColor();
                    Console.Write($"] {message.Message}{Environment.NewLine}");
                    WriteToLogFile(message.Message);
                }
            }
            else
            {
                if(logLevel == LogLevel.Warning)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Warning");
                    Console.ResetColor();
                    Console.Write("] ");
                    Console.Write($"{formatter(state, exception)}{Environment.NewLine}");
                    WriteToLogFile(formatter(state, exception));
                }
                else if (logLevel == LogLevel.Error)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Error");
                    Console.ResetColor();
                    Console.Write("] ");
                    Console.Write($"{formatter(state, exception)}{Environment.NewLine}");
                    WriteToLogFile(formatter(state, exception));
                }
                else
                {
                    Console.WriteLine(formatter(state, exception));
                    WriteToLogFile(formatter(state, exception));
                }         
            }
        }
    }

    private void WriteToLogFile(string message)
    {
        if (this.logFile != null)
        {
            File.AppendAllText(this.logFile, $"{message}{Environment.NewLine}");
        }
    }

    private ConsoleColor GetConsoleColorBasedOnChange(WhatIfChangeType changeType)
    {
        switch (changeType)
        {
            case WhatIfChangeType.Create:
                return ConsoleColor.Green;
            case WhatIfChangeType.Delete:
                return ConsoleColor.Red;
            case WhatIfChangeType.Deploy:
                return ConsoleColor.Blue;
            case WhatIfChangeType.Ignore:
                return ConsoleColor.DarkGray;
            case WhatIfChangeType.Modify:
                return ConsoleColor.Yellow;
            case WhatIfChangeType.NoChange:
                return ConsoleColor.Gray;
            case WhatIfChangeType.Unsupported:
                return ConsoleColor.DarkGray;
            case WhatIfChangeType.Unknown:
                return ConsoleColor.DarkGray;
            default:
                return ConsoleColor.DarkGray;
        }
    }
}