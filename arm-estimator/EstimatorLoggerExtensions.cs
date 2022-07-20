using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

internal static class EstimatorLoggerExtensions
{
    public static ILoggingBuilder AddEstimatorLogger(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, EstimatorLoggerProvider>());

        return builder;
    }
}
