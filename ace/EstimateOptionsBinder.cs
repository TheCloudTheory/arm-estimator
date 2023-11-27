using ACE.Output;
using ACE.WhatIf;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Text.Json;

namespace ACE;

internal class EstimateOptionsBinder : BinderBase<EstimateOptions>
{
    private readonly Option<DeploymentMode> mode;
    private readonly Option<int?> threshold;
    private readonly Option<FileInfo?> parameters;
    private readonly Option<CurrencyCode> currency;
    private readonly Option<bool> generateJsonOutput;
    private readonly Option<bool> shouldBeSilent;
    private readonly Option<bool> stdout;
    private readonly Option<bool> disableDetails;
    private readonly Option<string?> jsonOutputFilename;
    private readonly Option<bool> generateHtmlOutput;
    private readonly Option<IEnumerable<string>> inlineParameters;
    private readonly Option<bool> dryRunOnly;
    private readonly Option<string?> htmlOutputFilename;
    private readonly Option<OutputFormat> outputFormat;
    private readonly Option<bool> disableCache;
    private readonly Option<string?> terraformExecutable;
    private readonly Option<double> conversionRate;
    private readonly Option<CacheHandler> cacheHandler;
    private readonly Option<string?> cacheStorageAccountName;
    private readonly Option<string?> webhookUrl;
    private readonly Option<string?> webhookAuthorization;
    private readonly Option<string?> logFile;
    private readonly Option<FileInfo?> configurationFile;

    public EstimateOptionsBinder(Option<DeploymentMode> mode,
                                 Option<int?> threshold,
                                 Option<FileInfo?> parameters,
                                 Option<CurrencyCode> currency,
                                 Option<bool> generateJsonOutput,
                                 Option<bool> shouldBeSilent,
                                 Option<bool> stdout,
                                 Option<bool> disableDetailsOptions,
                                 Option<string?> jsonOutputFilenameOption,
                                 Option<bool> generateHtmlOutput,
                                 Option<IEnumerable<string>> inlineParameters,
                                 Option<bool> dryRunOnly,
                                 Option<string?> htmlOutputFilenameOption,
                                 Option<OutputFormat> outputFormat,
                                 Option<bool> disableCache,
                                 Option<string?> terraformExecutable,
                                 Option<double> conversionRateOption,
                                 Option<CacheHandler> cacheHandlerOption,
                                 Option<string?> cacheStorageAccountNameOption,
                                 Option<string?> webhookUrlOption,
                                 Option<string?> webhookAuthorizationOption,
                                 Option<string?> logFileOption,
                                 Option<FileInfo?> configurationFileOption)
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
    }

    protected override EstimateOptions GetBoundValue(BindingContext bindingContext)
    {
        if(this.configurationFile != null)
        {
            ValidateProperUse();

            var value = bindingContext.ParseResult.GetValueForOption(configurationFile) ?? throw new Exception("Configuration file is null");
            var configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(value.FullName), new JsonSerializerOptions()
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
                configuration.LogFile
                );
        }

        return new EstimateOptions(
            bindingContext.ParseResult.GetValueForOption(mode),
            bindingContext.ParseResult.GetValueForOption(threshold),
            bindingContext.ParseResult.GetValueForOption(parameters),
            bindingContext.ParseResult.GetValueForOption(currency),
            bindingContext.ParseResult.GetValueForOption(generateJsonOutput),
            bindingContext.ParseResult.GetValueForOption(shouldBeSilent),
            bindingContext.ParseResult.GetValueForOption(stdout),
            bindingContext.ParseResult.GetValueForOption(disableDetails),
            bindingContext.ParseResult.GetValueForOption(jsonOutputFilename),
            bindingContext.ParseResult.GetValueForOption(generateHtmlOutput),
            bindingContext.ParseResult.GetValueForOption(inlineParameters),
            bindingContext.ParseResult.GetValueForOption(dryRunOnly),
            bindingContext.ParseResult.GetValueForOption(htmlOutputFilename),
            bindingContext.ParseResult.GetValueForOption(outputFormat),
            bindingContext.ParseResult.GetValueForOption(disableCache),
            bindingContext.ParseResult.GetValueForOption(terraformExecutable),
            bindingContext.ParseResult.GetValueForOption(conversionRate),
            bindingContext.ParseResult.GetValueForOption(cacheHandler),
            bindingContext.ParseResult.GetValueForOption(cacheStorageAccountName),
            bindingContext.ParseResult.GetValueForOption(webhookUrl),
            bindingContext.ParseResult.GetValueForOption(webhookAuthorization),
            bindingContext.ParseResult.GetValueForOption(logFile)
            );
    }

    private void ValidateProperUse()
    {
        if(this.mode != null 
        || this.threshold != null 
        || this.parameters != null 
        || this.currency != null 
        || this.generateJsonOutput != null 
        || this.shouldBeSilent != null 
        || this.stdout != null 
        || this.disableDetails != null 
        || this.jsonOutputFilename != null 
        || this.generateHtmlOutput != null 
        || this.dryRunOnly != null 
        || this.htmlOutputFilename != null 
        || this.outputFormat != null 
        || this.disableCache != null 
        || this.terraformExecutable != null 
        || this.conversionRate != null 
        || this.cacheHandler != null 
        || this.cacheStorageAccountName != null 
        || this.webhookUrl != null
        || this.logFile != null)
        {
            throw new Exception("Cannot use both --configuration-file and other options besides --webhook-authorization and --inline.");
        }
    }
}