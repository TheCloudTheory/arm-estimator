using ACE.Output;
using ACE.WhatIf;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Text.Json;

namespace ACE;

internal class EstimateOptionsBinder(Option<DeploymentMode?> mode,
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
                             Option<bool?> optOutCheckingNewVersionOption,
                             Option<FileInfo[]?> mockedRetailAPIResponsePathOption,
                             Option<bool> debugOption,
                             Option<string?> userGeneratedWhatIfOption,
                             Option<bool?> generateMarkdownOutputOption,
                             Option<string?> markdownOutputFilenameOption,
                             Option<bool> forceUsingBicepCliOption) : BinderBase<EstimateOptions>
{
    private readonly Option<DeploymentMode?> mode = mode;
    private readonly Option<int?> threshold = threshold;
    private readonly Option<FileInfo?> parameters = parameters;
    private readonly Option<CurrencyCode?> currency = currency;
    private readonly Option<bool?> generateJsonOutput = generateJsonOutput;
    private readonly Option<bool?> shouldBeSilent = shouldBeSilent;
    private readonly Option<bool?> stdout = stdout;
    private readonly Option<bool?> disableDetails = disableDetailsOptions;
    private readonly Option<string?> jsonOutputFilename = jsonOutputFilenameOption;
    private readonly Option<bool?> generateHtmlOutput = generateHtmlOutput;
    private readonly Option<IEnumerable<string>> inlineParameters = inlineParameters;
    private readonly Option<bool?> dryRunOnly = dryRunOnly;
    private readonly Option<string?> htmlOutputFilename = htmlOutputFilenameOption;
    private readonly Option<OutputFormat?> outputFormat = outputFormat;
    private readonly Option<bool?> disableCache = disableCache;
    private readonly Option<string?> terraformExecutable = terraformExecutable;
    private readonly Option<double?> conversionRate = conversionRateOption;
    private readonly Option<CacheHandler?> cacheHandler = cacheHandlerOption;
    private readonly Option<string?> cacheStorageAccountName = cacheStorageAccountNameOption;
    private readonly Option<string?> webhookUrl = webhookUrlOption;
    private readonly Option<string?> webhookAuthorization = webhookAuthorizationOption;
    private readonly Option<string?> logFile = logFileOption;
    private readonly Option<FileInfo?> configurationFile = configurationFileOption;
    private readonly Option<bool?> optOutCheckingNewVersion = optOutCheckingNewVersionOption;
    private readonly Option<FileInfo[]?> mockedRetailAPIResponsePaths = mockedRetailAPIResponsePathOption;
    private readonly Option<bool> debug = debugOption;
    private readonly Option<string?> userGeneratedWhatIf = userGeneratedWhatIfOption;
    private readonly Option<bool?> generateMarkdownOutput = generateMarkdownOutputOption;
    private readonly Option<string?> markdownOutputFilename = markdownOutputFilenameOption;
    private readonly Option<bool> forceUsingBicepCli = forceUsingBicepCliOption;

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
                configuration.DisableVersionCheck,
                configuration.MockedRetailAPIResponsePaths,
                configuration.Debug,
                configuration.UserGeneratedWhatIf,
                configuration.GenerateMarkdownOutput,
                configuration.MarkdownOutputFilename,
                configuration.ForceUsingBicepCli
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
            bindingContext.ParseResult.GetValueForOption(optOutCheckingNewVersion) ?? Defaults.OptOutCheckingNewVersion,
            bindingContext.ParseResult.GetValueForOption(mockedRetailAPIResponsePaths),
            bindingContext.ParseResult.GetValueForOption(debug),
            bindingContext.ParseResult.GetValueForOption(userGeneratedWhatIf),
            bindingContext.ParseResult.GetValueForOption(generateMarkdownOutput) ?? Defaults.GenerateMarkdownOutput,
            bindingContext.ParseResult.GetValueForOption(markdownOutputFilename),
            bindingContext.ParseResult.GetValueForOption(forceUsingBicepCli)
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
        bindingContext.ParseResult.GetValueForOption(optOutCheckingNewVersion) != null ||
        bindingContext.ParseResult.GetValueForOption(mockedRetailAPIResponsePaths) != null ||
        bindingContext.ParseResult.GetValueForOption(userGeneratedWhatIf) != null ||
        bindingContext.ParseResult.GetValueForOption(generateMarkdownOutput) != null ||
        bindingContext.ParseResult.GetValueForOption(markdownOutputFilename) != null))
        {
            throw new Exception("Cannot use both --configuration-file and other options besides --webhook-authorization and --inline.");
        }
    }
}