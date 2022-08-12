﻿using Microsoft.Extensions.Logging;

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
            if(typeof(TState) == typeof(ChangeMessage))
            {
                if (state is ChangeMessage message)
                {
                    Console.Write("-> [");
                    Console.ForegroundColor = GetConsoleColorBasedOnChange(message.ChangeType);
                    Console.Write(message.ChangeType);
                    Console.ResetColor();
                    Console.Write($"] {message.Message}{Environment.NewLine}");
                }
            }
            else
            {
                Console.WriteLine(formatter(state, exception));
            }
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