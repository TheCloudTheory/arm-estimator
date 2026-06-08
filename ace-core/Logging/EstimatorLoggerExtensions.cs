using Microsoft.Extensions.Logging;

namespace ACE.Logging;

internal static class EstimatorLoggerExtensions
{
    public static void AddEstimatorMessage(this ILogger logger, string message, params object?[] args)
    {
        var formattedMessage = string.Format(message, args);
        logger.LogInformation("-> {message}", formattedMessage);
    }

    public static void AddDebugMessage(this ILogger logger, string message, bool isDebugEnabled, params object?[] args)
    {
        if (!isDebugEnabled)
        {
            return;
        }

        var formattedMessage = string.Format(message, args);
        logger.LogInformation("[DEBUG] {message}", formattedMessage);
    }
}
