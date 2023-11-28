using ACE.Output;
using ACE.WhatIf;

namespace ACE;

internal class Defaults 
{
    public const DeploymentMode Mode = DeploymentMode.Incremental;
    public const CurrencyCode Currency = CurrencyCode.USD;
    public const bool ShouldGenerateJsonOutput = false;
    public const bool ShouldBeSilent = false;
    public const bool Stdout = false;
    public const bool DisableDetailedMetrics = false;
    public const bool ShouldGenerateHtmlOutput = false;
    public const bool DryRunOnly = false;
    public const OutputFormat Output = OutputFormat.Default;
    public const bool DisableCache = false;
    public const double ConversionRate = 1.0;
    public const CacheHandler Cache = CacheHandler.Local;
    public const bool OptOutCheckingNewVersion = false;
}