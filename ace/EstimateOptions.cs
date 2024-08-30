using ACE.Output;
using ACE.WhatIf;

namespace ACE;

internal class EstimateOptions(DeploymentMode mode,
                       int? threshold,
                       FileInfo? parametersFile,
                       CurrencyCode currency,
                       bool shouldGenerateOutput,
                       bool shouldBeSilent,
                       bool stdout,
                       bool disableDetailedMetrics,
                       string? jsonOutputFilename,
                       bool shouldGenerateHtmlOutput,
                       IEnumerable<string>? inlineParameters,
                       bool dryRunOnly,
                       string? htmlOutputFilename,
                       OutputFormat outputFormat,
                       bool disableCache,
                       string? terraformExecutable,
                       double conversionRate,
                       CacheHandler cacheHandler,
                       string? cacheHandlerStorageAccountName,
                       string? webhookUrl,
                       string? webhookAuthorization,
                       string? logFile,
                       bool optOutCheckingNewVersion,
                       FileInfo[]? mockedRetailAPIResponsePaths,
                       bool debug,
                       string? userGeneratedWhatIf,
                       bool generateMarkdownOutput,
                       string? markdownOutputFilename,
                       bool forceUsingBicepCli)
{
    public DeploymentMode Mode { get; } = mode;
    public int? Threshold { get; } = threshold;
    public FileInfo? ParametersFile { get; } = parametersFile;
    public CurrencyCode Currency { get; } = currency;
    public bool ShouldGenerateJsonOutput { get; } = shouldGenerateOutput;
    public bool ShouldBeSilent { get; } = shouldBeSilent;
    public bool Stdout { get; } = stdout;
    public bool DisableDetailedMetrics { get; } = disableDetailedMetrics;
    public string? JsonOutputFilename { get; } = jsonOutputFilename;
    public bool ShouldGenerateHtmlOutput { get; } = shouldGenerateHtmlOutput;
    public IEnumerable<string>? InlineParameters { get; } = inlineParameters;
    public bool DryRunOnly { get; } = dryRunOnly;
    public string? HtmlOutputFilename { get; } = htmlOutputFilename;
    public OutputFormat OutputFormat { get; } = outputFormat;
    public bool DisableCache { get; } = disableCache;
    public string? TerraformExecutable { get; } = terraformExecutable;
    public double ConversionRate { get; } = conversionRate;
    public CacheHandler CacheHandler { get; } = cacheHandler;
    public string? CacheHandlerStorageAccountName { get; } = cacheHandlerStorageAccountName;
    public string? WebhookUrl { get; } = webhookUrl;
    public string? WebhookAuthorization { get; } = webhookAuthorization;
    public string? LogFile { get; } = logFile;
    public bool OptOutCheckingNewVersion { get; } = optOutCheckingNewVersion;
    public FileInfo[]? MockedRetailAPIResponsePaths { get; } = mockedRetailAPIResponsePaths;
    public bool Debug { get; } = debug;
    public string? UserGeneratedWhatIfFile { get; } = userGeneratedWhatIf;
    public bool ShouldGenerateMarkdownOutput { get; } = generateMarkdownOutput;
    public string? MarkdownOutputFilename { get; } = markdownOutputFilename;
    public bool ForceUsingBicepCli { get; } = forceUsingBicepCli;
}
