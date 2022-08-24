using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

internal static class EstimatorLoggerExtensions
{
    public static ILoggingBuilder AddEstimatorLogger(this ILoggingBuilder builder, bool isSilent)
    {
        builder.AddConfiguration();
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, EstimatorLoggerProvider>((provider) => new EstimatorLoggerProvider(isSilent)));

        return builder;
    }

    public static void AddEstimatorMessage(this ILogger logger, string message, params object?[] args)
    {
        var formattedMessage = string.Format(message, args);
        logger.LogInformation("-> {message}", formattedMessage);
    }

    public static void AddEstimatorMessageSubsection(this ILogger logger, string message, params object?[] args)
    {
        var formattedMessage = string.Format(message, args);
        logger.LogInformation("   \\--- {message}", formattedMessage);
    }

    public static void AddEstimatorMessageSensibleToChange(this ILogger logger, WhatIfChangeType? changeType, string message, params object?[] args)
    {
        var formattedMessage = string.Format(message, args);
        logger.Log<ChangeMessage>(LogLevel.Information, new EventId(), new ChangeMessage(changeType ?? WhatIfChangeType.Unknown, formattedMessage), null, (val1, val2) => formattedMessage);
    }
}
