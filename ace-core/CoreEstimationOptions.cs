using ACE.Caching;
using ACE.WhatIf;

namespace ACE.Core;

/// <summary>
/// Lightweight options record for the ACE.Core library. Contains only the fields
/// required for cost computation — no CLI, output, or file-path concerns.
/// </summary>
public record CoreEstimationOptions
{
    /// <summary>ISO 4217 currency code to use for Retail API pricing queries. Defaults to USD.</summary>
    public CurrencyCode Currency { get; init; } = CurrencyCode.USD;

    /// <summary>
    /// Multiplier applied to every retail price before it is returned. Useful for
    /// enterprise discount modelling. Defaults to 1.0 (no adjustment).
    /// </summary>
    public double ConversionRate { get; init; } = 1.0;

    /// <summary>VM capabilities cache storage back-end. Defaults to local file cache.</summary>
    public CacheHandler CacheHandler { get; init; } = CacheHandler.Local;

    /// <summary>
    /// Azure Storage account name used when <see cref="CacheHandler"/> is
    /// <see cref="CacheHandler.AzureStorage"/>. Ignored otherwise.
    /// </summary>
    public string? CacheHandlerStorageAccountName { get; init; }

    /// <summary>
    /// Paths to JSON files that substitute real Retail API responses. Useful in
    /// offline environments or tests. When set, no network calls are made.
    /// </summary>
    public FileInfo[]? MockedRetailAPIResponsePaths { get; init; }

    /// <summary>Emit verbose debug log messages during estimation. Defaults to false.</summary>
    public bool Debug { get; init; }
}
