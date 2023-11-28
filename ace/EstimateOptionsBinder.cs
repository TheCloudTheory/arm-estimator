using ACE.Output;
using ACE.WhatIf;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Text.Json;

namespace ACE;

internal class EstimateOptionsBinder : BinderBase<EstimateOptions>
{
    private readonly Option<DeploymentMode?> mode;
    private readonly Option<int?> threshold;
    private readonly Option<FileInfo?> parameters;
    private readonly Option<CurrencyCode?> currency;
    private readonly Option<bool?> generateJsonOutput;
    private readonly Option<bool?> shouldBeSilent;
    private readonly Option<bool?> stdout;
    private readonly Option<bool?> disableDetails;
    private readonly Option<string?> jsonOutputFilename;
    private readonly Option<bool?> generateHtmlOutput;
    private readonly Option<IEnumerable<string>> inlineParameters;
    private readonly Option<bool?> dryRunOnly;
    private readonly Option<string?> htmlOutputFilename;
    private readonly Option<OutputFormat?> outputFormat;
    private readonly Option<bool?> disableCache;
    private readonly Option<string?> terraformExecutable;
    private readonly Option<double?> conversionRate;
    private readonly Option<CacheHandler?> cacheHandler;
    private readonly Option<string?> cacheStorageAccountName;
    private readonly Option<string?> webhookUrl;
    private readonly Option<string?> webhookAuthorization;
    private readonly Option<string?> logFile;
    private readonly Option<FileInfo?> configurationFile;
    private readonly Option<bool?> optOutCheckingNewVersion;

    public EstimateOptionsBinder(Option<DeploymentMode?> mode,
                                 Option<int?> threshold,
                                 Option<FileInfo?> parameters,
                                 Option<CurrencyCode?> currency,
                                 Option<bool?> generateJsonOutput,
                                 Option<bool?> shouldBeSilent,
                                 Option<bool?> stdout,
                                 Option<bool?> disableDetailsOptions,
                                 Option<string?> jsonOutputFilenameOption,
                                 Option<bool?> generateHtmlOutput,
                                 Option<IEnumerable<string>> inlineParameters,
                                 Option<bool?> dryRunOnly,
                                 Option<string?> htmlOutputFilenameOption,
                                 Option<OutputFormat?> outputFormat,
                                 Option<bool?> disableCache,
                                 Option<string?> terraformExecutable,
                                 Option<double?> conversionRateOption,
                                 Option<CacheHandler?> cacheHandlerOption,
                                 Option<string?> cacheStorageAccountNameOption,
                                 Option<string?> webhookUrlOption,
                                 Option<string?> webhookAuthorizationOption,
                                 Option<string?> logFileOption,
                                 Option<FileInfo?> configurationFileOption,
                                 Option<bool?> optOutCheckingNewVersionOption)
    {
        this.mode = mode;
        this.threshold = threshold;
        this.parameters = parameters;
        this.currency = currency;
        this.generateJsonOutput = generateJsonOutput;
        this.shouldBeSilent = shouldBeSilent;
        this.stdout = stdout;
        this.disableDetails = disableDetailsOptions;
        this.jsonOutputFilename = jsonOutputFilenameOption;
        this.generateHtmlOutput = generateHtmlOutput;
        this.inlineParameters = inlineParameters;
        this.dryRunOnly = dryRunOnly;
        this.htmlOutputFilename = htmlOutputFilenameOption;
        this.outputFormat = outputFormat;
        this.disableCache = disableCache;
        this.terraformExecutable = terraformExecutable;
        this.conversionRate = conversionRateOption;
        this.cacheHandler = cacheHandlerOption;
        this.cacheStorageAccountName = cacheStorageAccountNameOption;
        this.webhookUrl = webhookUrlOption;
        this.webhookAuthorization = webhookAuthorizationOption;
        this.logFile = logFileOption;
        this.configurationFile = configurationFileOption;
        this.optOutCheckingNewVersion = optOutCheckingNewVersionOption;
    }

    protected override EstimateOptions GetBoundValue(BindingContext bindingContext)
    {
        var file = bindingContext.ParseResult.GetValueForOption(configurationFile);
        if(file != null)
        {
            ValidateProperUse(bindingContext);

            var configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(file.FullName), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new Exception("Couldn't deserialize configuration file");

            return new EstimateOptions(
                configuration.Mode,
                configuration.Threshold,
                configuration.Parameters,
                configuration.Currency,
                configuration.GenerateJsonOutput,
                configuration.Silent,
                configuration.Stdout,
                configuration.DisableDetailedMetrics,
                configuration.JsonOutputFilename,
                configuration.GenerateHtmlOutput,
                bindingContext.ParseResult.GetValueForOption(inlineParameters),
                configuration.DryRun,
                configuration.HtmlOutputFilename,
                configuration.OutputFormat,
                configuration.DisableCache,
                configuration.TfExecutable,
                configuration.ConversionRate,
                configuration.CacheHandler,
                configuration.CacheStorageAccountName,
                configuration.WebhookUrl,
                bindingContext.ParseResult.GetValueForOption(webhookAuthorization),
                configuration.LogFile,
                configuration.DisableVersionCheck
                );
        }

        return new EstimateOptions(
            bindingContext.ParseResult.GetValueForOption(mode) ?? Defaults.Mode,
            bindingContext.ParseResult.GetValueForOption(threshold),
            bindingContext.ParseResult.GetValueForOption(parameters),
            bindingContext.ParseResult.GetValueForOption(currency) ?? Defaults.Currency,
            bindingContext.ParseResult.GetValueForOption(generateJsonOutput) ?? Defaults.ShouldGenerateJsonOutput,
            bindingContext.ParseResult.GetValueForOption(shouldBeSilent) ?? Defaults.ShouldBeSilent,
            bindingContext.ParseResult.GetValueForOption(stdout) ?? Defaults.Stdout,
            bindingContext.ParseResult.GetValueForOption(disableDetails) ?? Defaults.DisableDetailedMetrics,
            bindingContext.ParseResult.GetValueForOption(jsonOutputFilename),
            bindingContext.ParseResult.GetValueForOption(generateHtmlOutput) ?? Defaults.ShouldGenerateHtmlOutput,
            bindingContext.ParseResult.GetValueForOption(inlineParameters),
            bindingContext.ParseResult.GetValueForOption(dryRunOnly) ?? Defaults.DryRunOnly,
            bindingContext.ParseResult.GetValueForOption(htmlOutputFilename),
            bindingContext.ParseResult.GetValueForOption(outputFormat) ?? Defaults.Output,
            bindingContext.ParseResult.GetValueForOption(disableCache) ?? Defaults.DisableCache,
            bindingContext.ParseResult.GetValueForOption(terraformExecutable),
            bindingContext.ParseResult.GetValueForOption(conversionRate) ?? Defaults.ConversionRate,
            bindingContext.ParseResult.GetValueForOption(cacheHandler) ?? Defaults.Cache,
            bindingContext.ParseResult.GetValueForOption(cacheStorageAccountName),
            bindingContext.ParseResult.GetValueForOption(webhookUrl),
            bindingContext.ParseResult.GetValueForOption(webhookAuthorization),
            bindingContext.ParseResult.GetValueForOption(logFile),
            bindingContext.ParseResult.GetValueForOption(optOutCheckingNewVersion) ?? Defaults.OptOutCheckingNewVersion
            );
    }

    private void ValidateProperUse(BindingContext bindingContext)
    {
        if(bindingContext.ParseResult.GetValueForOption(configurationFile) != null && 
        (bindingContext.ParseResult.GetValueForOption(mode) != null ||
        bindingContext.ParseResult.GetValueForOption(threshold) != null ||
        bindingContext.ParseResult.GetValueForOption(parameters) != null ||
        bindingContext.ParseResult.GetValueForOption(currency) != null ||
        bindingContext.ParseResult.GetValueForOption(generateJsonOutput) != null ||
        bindingContext.ParseResult.GetValueForOption(shouldBeSilent) != null ||
        bindingContext.ParseResult.GetValueForOption(stdout) != null ||
        bindingContext.ParseResult.GetValueForOption(disableDetails) != null ||
        bindingContext.ParseResult.GetValueForOption(jsonOutputFilename) != null ||
        bindingContext.ParseResult.GetValueForOption(generateHtmlOutput) != null ||
        bindingContext.ParseResult.GetValueForOption(dryRunOnly) != null ||
        bindingContext.ParseResult.GetValueForOption(htmlOutputFilename) != null ||
        bindingContext.ParseResult.GetValueForOption(outputFormat) != null ||
        bindingContext.ParseResult.GetValueForOption(disableCache) != null ||
        bindingContext.ParseResult.GetValueForOption(terraformExecutable) != null ||
        bindingContext.ParseResult.GetValueForOption(conversionRate) != null ||
        bindingContext.ParseResult.GetValueForOption(cacheHandler) != null ||
        bindingContext.ParseResult.GetValueForOption(cacheStorageAccountName) != null ||
        bindingContext.ParseResult.GetValueForOption(webhookUrl) != null ||
        bindingContext.ParseResult.GetValueForOption(logFile) != null ||
        bindingContext.ParseResult.GetValueForOption(optOutCheckingNewVersion) != null))
        {
            throw new Exception("Cannot use both --configuration-file and other options besides --webhook-authorization and --inline.");
        }
    }
}