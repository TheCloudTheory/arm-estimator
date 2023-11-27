using ACE.Output;
using ACE.WhatIf;

namespace ACE;

internal class Configuration
{
    public DeploymentMode Mode { get; set; }
    public int? Threshold { get; set; }
    public FileInfo? Parameters { get; set; }
    public CurrencyCode Currency { get; set; }
    public bool GenerateJsonOutput { get; set; }
    public bool Silent { get; set; }
    public bool Stdout { get; set; }
    public bool DisableDetailedMetrics { get; set; }
    public string? JsonOutputFilename { get; set; }
    public bool GenerateHtmlOutput { get; set; }
    public bool DryRun { get; set; }
    public string? HtmlOutputFilename { get; set; }
    public OutputFormat OutputFormat { get; set; }
    public bool DisableCache { get; set; }
    public string? TfExecutable { get; set; }
    public double ConversionRate { get; set; }
    public CacheHandler CacheHandler { get; set; }
    public string? CacheStorageAccountName { get; set; }
    public string? WebhookUrl { get; set; }
    public string? LogFile { get; set; }
}