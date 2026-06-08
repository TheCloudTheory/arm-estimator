using ACE.Output;
using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ACE.Core;

/// <summary>
/// Public entry point for the ACE.Core cost-estimation engine.
/// </summary>
/// <example>
/// <code>
/// var service = new EstimationService();
/// var changes = new[]
/// {
///     new WhatIfChange
///     {
///         resourceId = "/subscriptions/…/resourceGroups/…/providers/Microsoft.KeyVault/vaults/my-vault",
///         changeType = WhatIfChangeType.Create,
///         after = new WhatIfAfterBeforeChange { location = "eastus", properties = … }
///     }
/// };
/// var result = await service.EstimateAsync(changes);
/// Console.WriteLine($"Total monthly cost: {result.TotalCost.OriginalValue} {result.Currency}");
/// </code>
/// </example>
public sealed class EstimationService
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initialises the service with an optional logger. Defaults to
    /// <see cref="NullLogger"/> when no logger is provided.
    /// </summary>
    public EstimationService(ILogger? logger = null)
    {
        _logger = logger ?? NullLogger.Instance;
    }

    /// <summary>
    /// Estimates the monthly cost for the supplied set of ARM resource changes.
    /// </summary>
    /// <param name="changes">
    /// Array of <see cref="WhatIfChange"/> objects describing the resources to
    /// price. Each entry must have a valid <c>resourceId</c> and a non-null
    /// <c>after</c> (or <c>before</c> for deletes) with a <c>location</c>.
    /// </param>
    /// <param name="options">
    /// Optional estimation options such as currency, conversion rate, and mock
    /// data paths. When null, defaults (<see cref="CoreEstimationOptions"/>) are used.
    /// </param>
    /// <param name="usagePatterns">
    /// Optional dictionary of ACE usage-pattern overrides keyed by pattern name.
    /// Maps to <c>metadata.aceUsagePatterns</c> in an ARM template.
    /// </param>
    /// <param name="token">Cancellation token.</param>
    public async Task<EstimationOutput> EstimateAsync(
        WhatIfChange[] changes,
        CoreEstimationOptions? options = null,
        IDictionary<string, string>? usagePatterns = null,
        CancellationToken token = default)
    {
        options ??= new CoreEstimationOptions();

        var template = new TemplateSchema
        {
            Metadata = usagePatterns is { Count: > 0 }
                ? new MetadataSchema { UsagePatterns = usagePatterns }
                : null
        };

        var formatter = new NullOutputFormatter();

        using var processor = new WhatIfProcessor(
            _logger,
            changes,
            template,
            options,
            formatter,
            token);

        return await processor.ProcessAsync(token);
    }
}
