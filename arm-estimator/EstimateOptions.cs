﻿using ACE.Output;
using ACE.WhatIf;

namespace ACE;

internal class EstimateOptions
{
    public EstimateOptions(DeploymentMode mode,
                           int threshold,
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
                           OutputFormat outputFormat)
    {
        Mode = mode;
        Threshold = threshold;
        ParametersFile = parametersFile;
        Currency = currency;
        ShouldGenerateJsonOutput = shouldGenerateOutput;
        ShouldBeSilent = shouldBeSilent;
        Stdout = stdout;
        DisableDetailedMetrics = disableDetailedMetrics;
        JsonOutputFilename = jsonOutputFilename;
        ShouldGenerateHtmlOutput = shouldGenerateHtmlOutput;
        InlineParameters = inlineParameters;
        DryRunOnly = dryRunOnly;
        HtmlOutputFilename = htmlOutputFilename;
        OutputFormat = outputFormat;
    }

    public DeploymentMode Mode { get; }
    public int Threshold { get; }
    public FileInfo? ParametersFile { get; }
    public CurrencyCode Currency { get; }
    public bool ShouldGenerateJsonOutput { get; }
    public bool ShouldBeSilent { get; }
    public bool Stdout { get; }
    public bool DisableDetailedMetrics { get; }
    public string? JsonOutputFilename { get; }
    public bool ShouldGenerateHtmlOutput { get; }
    public IEnumerable<string>? InlineParameters { get; }
    public bool DryRunOnly { get; }
    public string? HtmlOutputFilename { get; }
    public OutputFormat OutputFormat { get; }
}
