using ACE.Output;
using ACE.WhatIf;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    protected override EstimateOptions GetBoundValue(BindingContext bindingContext)
    {
        var file = bindingContext.ParseResult.GetValueForOption(configurationFile);
        if(file != null)
        {
            ValidateProperUse(bindingContext);

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            
            var configuration = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(file.FullName), options) ?? throw new Exception("Couldn't deserialize configuration file");

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
            bindingContext.ParseResult.GetValueForOption(forceUsingBicepCliOption)
            );
    }

    private void ValidateProperUse(BindingContext bindingContext)
    {
        var options = new Dictionary<string, object?>
        {
            { mode.Name, bindingContext.ParseResult.GetValueForOption(mode) },
            { threshold.Name, bindingContext.ParseResult.GetValueForOption(threshold) },
            { parameters.Name, bindingContext.ParseResult.GetValueForOption(parameters) },
            { currency.Name, bindingContext.ParseResult.GetValueForOption(currency) },
            { generateJsonOutput.Name, bindingContext.ParseResult.GetValueForOption(generateJsonOutput) },
            { shouldBeSilent.Name, bindingContext.ParseResult.GetValueForOption(shouldBeSilent) },
            { stdout.Name, bindingContext.ParseResult.GetValueForOption(stdout) },
            { disableDetails.Name, bindingContext.ParseResult.GetValueForOption(disableDetails) },
            { jsonOutputFilename.Name, bindingContext.ParseResult.GetValueForOption(jsonOutputFilename) },
            { generateHtmlOutput.Name, bindingContext.ParseResult.GetValueForOption(generateHtmlOutput) },
            { outputFormat.Name, bindingContext.ParseResult.GetValueForOption(outputFormat) },
            { disableCache.Name, bindingContext.ParseResult.GetValueForOption(disableCache) },
            { terraformExecutable.Name, bindingContext.ParseResult.GetValueForOption(terraformExecutable) },
            { conversionRate.Name, bindingContext.ParseResult.GetValueForOption(conversionRate) },
            { cacheHandler.Name, bindingContext.ParseResult.GetValueForOption(cacheHandler) },
            { cacheStorageAccountName.Name, bindingContext.ParseResult.GetValueForOption(cacheStorageAccountName) },
            { webhookUrl.Name, bindingContext.ParseResult.GetValueForOption(webhookUrl) },
            { logFile.Name, bindingContext.ParseResult.GetValueForOption(logFile) },
            { optOutCheckingNewVersion.Name, bindingContext.ParseResult.GetValueForOption(optOutCheckingNewVersion) },
            { mockedRetailAPIResponsePaths.Name, GetNullFileInfoIfNull(bindingContext.ParseResult.GetValueForOption(mockedRetailAPIResponsePaths)) },
            { userGeneratedWhatIf.Name, bindingContext.ParseResult.GetValueForOption(userGeneratedWhatIf) },
            { generateMarkdownOutput.Name, bindingContext.ParseResult.GetValueForOption(generateMarkdownOutput) },
            { markdownOutputFilename.Name, bindingContext.ParseResult.GetValueForOption(markdownOutputFilename) },
        };
        
        if (bindingContext.ParseResult.GetValueForOption(configurationFile) == null) return;
        
        var optionsWithNonNullValue = options.Where(o => o.Value != null).ToArray();
        if (optionsWithNonNullValue.Length == 0) return;
        
        var parsedNonNullableOptions = string.Join(", ", optionsWithNonNullValue.Select(o => $"[{o.Key}]:[{o.Value}]"));
        
        throw new Exception(
            $"Cannot use both --configuration-file and other options besides --webhook-authorization and --inline. Those options are: {parsedNonNullableOptions}");
        
    }

    /// <summary>
    /// As System.CommandLine tends to parse a nullable array as an empty array,
    /// this method implement a safeguard to overcome that issue. 
    /// </summary>
    private FileInfo[]? GetNullFileInfoIfNull(FileInfo[]? fileInfos)
    {
        if (fileInfos == null) return null;
        return fileInfos.Length == 0 ? null : fileInfos;
    }
}