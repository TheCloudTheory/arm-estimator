using ACE.Compilation;
using ACE.Output;
using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ACE;

public partial class Program
{
    private static string? GetInformationalVersion() => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    private static readonly CancellationTokenSource _cancellationTokenSource = new();

    public static int Main(string[] args)
    {
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Console.WriteLine("Received cancellation signal, exiting...");
            _cancellationTokenSource.Cancel();
            eventArgs.Cancel = true;
        };

        var templateFileArg = new Argument<FileInfo>("template-file", "Template file to analyze");
        var susbcriptionIdArg = new Argument<string>("subscription-id", "Subscription ID");
        var resourceGroupArg = new Argument<string>("resource-group", "Resource group name");
        var managementGroupArg = new Argument<string>("management-group", "Management group name");
        var locationArg = new Argument<string>("location", "Deployment location");

        var deploymentModeOption = new Option<DeploymentMode?>("--mode", "Deployment mode");
        var thresholdOption = new Option<int?>("--threshold", "Estimation threshold");
        var parametersOption = new Option<FileInfo?>("--parameters", "Path to a file containing values of template parameters");
        var currencyOption = new Option<CurrencyCode?>("--currency", "Currency code");
        var jsonOutputOption = new Option<bool?>("--generate-json-output", "Should generate JSON output");
        var silentOption = new Option<bool?>("--silent", "Mute all logs");
        var stdoutOption = new Option<bool?>("--stdout", "Redirects JSON output to stdout");
        var disableDetailsOption = new Option<bool?>("--disable-detailed-metrics", "Disables reporting of detailed metrics");
        var jsonOutputFilenameOption = new Option<string?>("--json-output-filename", "Sets JSON output filename");
        var htmlOutputOption = new Option<bool?>("--generate-html-output", "Should generate HTML output");
        var inlineOptions = new Option<IEnumerable<string>>("--inline", "List of inline parameters");
        var dryRunOption = new Option<bool?>("--dry-run", "Run template validation only");
        var htmlOutputFilenameOption = new Option<string?>("--html-output-filename", "Sets HTML output filename");
        var outputFormatOption = new Option<OutputFormat?>("--output-format", "Sets output format");
        var disableCacheOption = new Option<bool?>("--disable-cache", "Disables in-built cache mechanism");
        var terraformExecutableOption = new Option<string?>("--tf-executable", "Provide path to Terraform executable. If omitted, ACE will try to find it in PATH");
        var conversionRateOption = new Option<double?>("--conversion-rate", "Conversion rate from USD to selected currency.");
        var cacheHandlerOption = new Option<CacheHandler?>("--cache-handler", "Selected cache handler to be used to store cached data");
        var cacheStorageAccountNameOption = new Option<string?>("--cache-storage-account-name", "Name of Azure Storage account to be used as cache storage. Required, if cache handler is set to AzureStorage");
        var webhookUrlOption = new Option<string?>("--webhook-url", "Webhook URL to be used for sending estimation result");
        var webhookAuthorizationOption = new Option<string?>("--webhook-authorization", "Webhook Authorization header value");
        var logFileOption = new Option<string?>("--log-file", "Path to a log file");
        var configurationFileOption = new Option<FileInfo?>("--configuration-file", "Path to configuration file for ACE");
        var optOutCheckingNewVersionOption = new Option<bool?>("--disable-version-check", "Whether to disable checking for new version of ACE");
        var retailAPIResponsePathOption = new Option<FileInfo[]?>("--mocked-retail-api-response-path", "Path to a file containing mocked Retail API response. Used for testing purposes only.");
        var debugOption = new Option<bool>("--debug", "Enables verbose logging");
        var userGeneratedWhatIfOption = new Option<string?>("--what-if-file", "Path to a file containing user-generated WhatIf response");
        var markdownOutputOption = new Option<bool?>("--generate-markdown-output", "Should generate Markdown output");
        var markdownOutputFilenameOption = new Option<string?>("--markdown-output-filename", "Sets Markdown output filename");
        var forceUsingBicepCliOption = new Option<bool>("--force-bicep-cli", () => false, "Force using Bicep CLI in case ACE couldn't correctly fallback after Azure CLI fails.");

        try
        {
            var rootCommand = new RootCommand("ACE (Azure Cost Estimator)");

            rootCommand.AddGlobalOption(deploymentModeOption);
            rootCommand.AddGlobalOption(thresholdOption);
            rootCommand.AddGlobalOption(parametersOption);
            rootCommand.AddGlobalOption(currencyOption);
            rootCommand.AddGlobalOption(jsonOutputOption);
            rootCommand.AddGlobalOption(silentOption);
            rootCommand.AddGlobalOption(stdoutOption);
            rootCommand.AddGlobalOption(disableDetailsOption);
            rootCommand.AddGlobalOption(jsonOutputFilenameOption);
            rootCommand.AddGlobalOption(htmlOutputOption);
            rootCommand.AddGlobalOption(inlineOptions);
            rootCommand.AddGlobalOption(dryRunOption);
            rootCommand.AddGlobalOption(htmlOutputFilenameOption);
            rootCommand.AddGlobalOption(outputFormatOption);
            rootCommand.AddGlobalOption(disableCacheOption);
            rootCommand.AddGlobalOption(terraformExecutableOption);
            rootCommand.AddGlobalOption(conversionRateOption);
            rootCommand.AddGlobalOption(cacheHandlerOption);
            rootCommand.AddGlobalOption(cacheStorageAccountNameOption);
            rootCommand.AddGlobalOption(webhookUrlOption);
            rootCommand.AddGlobalOption(webhookAuthorizationOption);
            rootCommand.AddGlobalOption(logFileOption);
            rootCommand.AddGlobalOption(configurationFileOption);
            rootCommand.AddGlobalOption(optOutCheckingNewVersionOption);
            rootCommand.AddGlobalOption(retailAPIResponsePathOption);
            rootCommand.AddGlobalOption(debugOption);
            rootCommand.AddGlobalOption(userGeneratedWhatIfOption);
            rootCommand.AddGlobalOption(markdownOutputOption);
            rootCommand.AddGlobalOption(markdownOutputFilenameOption);
            rootCommand.AddGlobalOption(forceUsingBicepCliOption);

            rootCommand.AddArgument(templateFileArg);
            rootCommand.AddArgument(susbcriptionIdArg);
            rootCommand.AddArgument(resourceGroupArg);

            var binder = new EstimateOptionsBinder(
                    deploymentModeOption,
                    thresholdOption,
                    parametersOption,
                    currencyOption,
                    jsonOutputOption,
                    silentOption,
                    stdoutOption,
                    disableDetailsOption,
                    jsonOutputFilenameOption,
                    htmlOutputOption,
                    inlineOptions,
                    dryRunOption,
                    htmlOutputFilenameOption,
                    outputFormatOption,
                    disableCacheOption,
                    terraformExecutableOption,
                    conversionRateOption,
                    cacheHandlerOption,
                    cacheStorageAccountNameOption,
                    webhookUrlOption,
                    webhookAuthorizationOption,
                    logFileOption,
                    configurationFileOption,
                    optOutCheckingNewVersionOption,
                    retailAPIResponsePathOption,
                    debugOption,
                    userGeneratedWhatIfOption,
                    markdownOutputOption,
                    markdownOutputFilenameOption,
                    forceUsingBicepCliOption
            );

            rootCommand.SetHandler(async (file, subscription, resourceGroup, options) =>
            {
                var result = await Estimate(file, subscription, resourceGroup, null, options, CommandType.ResourceGroup);
                if (result.ExitCode != 0)
                {
                    throw new Exception(result.ErrorMessage);
                }
            },
                templateFileArg,
                susbcriptionIdArg,
                resourceGroupArg,
                binder);

            var subscriptionCommand = new Command("sub", "Calculate estimation for subscription");
            subscriptionCommand.AddArgument(templateFileArg);
            subscriptionCommand.AddArgument(susbcriptionIdArg);
            subscriptionCommand.AddArgument(locationArg);

            subscriptionCommand.SetHandler(async (file, subscription, location, options) =>
            {
                var result = await Estimate(file, subscription, null, location, options, CommandType.Subscription);
                if (result.ExitCode != 0)
                {
                    throw new Exception(result.ErrorMessage);
                }
            },
                templateFileArg,
                susbcriptionIdArg,
                locationArg,
                binder);

            var managementGroupCommand = new Command("mg", "Calculate estimation for management group");
            managementGroupCommand.AddArgument(templateFileArg);
            managementGroupCommand.AddArgument(managementGroupArg);
            managementGroupCommand.AddArgument(locationArg);

            managementGroupCommand.SetHandler(async (file, managementGroup, location, options) =>
            {
                var result = await Estimate(file, managementGroup, null, location, options, CommandType.ManagementGroup);
                if (result.ExitCode != 0)
                {
                    throw new Exception(result.ErrorMessage);
                }
            },
                templateFileArg,
                managementGroupArg,
                locationArg,
                binder);

            var tenantCommand = new Command("tenant", "Calculate estimation for tenant");
            tenantCommand.AddArgument(templateFileArg);
            tenantCommand.AddArgument(locationArg);

            tenantCommand.SetHandler(async (file, location, options) =>
            {
                var result = await Estimate(file, "<tenant>", null, location, options, CommandType.Tenant);
                if (result.ExitCode != 0)
                {
                    throw new Exception(result.ErrorMessage);
                }
            },
                templateFileArg,
                locationArg,
                binder);

            rootCommand.AddCommand(subscriptionCommand);
            rootCommand.AddCommand(managementGroupCommand);
            rootCommand.AddCommand(tenantCommand);

            var parser = new CommandLineBuilder(rootCommand)
                .UseDefaults()
                .Build();

            return parser.Invoke(args);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An error occurred: {0}", ex.Message);
            return 1;
        }
    }

    private static async Task<ApplicationResult> Estimate(FileInfo templateFile,
                                            string scopeId,
                                            string? resourceGroupName,
                                            string? location,
                                            EstimateOptions options,
                                            CommandType commandType)
    {
        using (var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddEstimatorLogger(options.ShouldBeSilent, options.LogFile);
        }))
        {
            var logger = loggerFactory.CreateLogger<Program>();

            await DisplayWelcomeScreenAsync(logger, options.OptOutCheckingNewVersion);
            DisplayUsedSettings(templateFile, scopeId, resourceGroupName, logger, options, commandType);

            var template = GetTemplate(templateFile, options.TerraformExecutable, options.ForceUsingBicepCli, logger, out var templateType);
            if (template == null)
            {
                var error = "There was a problem with processing template.";
                logger.LogError("{error}", error);

                return new ApplicationResult(1,error);
            }

            var parameters = "{}";
            if (options.ParametersFile != null)
            {   
                var isUsingBicepparamFile = options.ParametersFile.FullName.EndsWith(".bicepparam");
                var fileContent = isUsingBicepparamFile ? new BicepCompiler(options.ForceUsingBicepCli, logger).CompileBicepparam(options.ParametersFile, _cancellationTokenSource.Token) : File.ReadAllText(options.ParametersFile.FullName);
                if (fileContent == null)
                {
                    var error = $"Couldn't read parameters file {options.ParametersFile.FullName}";
                    logger.LogError("{error}", error);
                    
                    return new ApplicationResult(1, error);
                }

                parameters = WhitespacesRegex().Replace(fileContent, string.Empty);
            }

            TemplateParser? parser = null;
            if (templateType == TemplateType.ArmTemplateOrBicep)
            {
                try
                {
                    parser = new TemplateParser(template, parameters, options.InlineParameters, scopeId, resourceGroupName, logger);
                }
                catch (JsonException ex)
                {
                    var error = $"Couldn't parse the following template - {template}. Error: { ex.Message}";
                    logger.LogError("{error}", error);
                    
                    return new ApplicationResult(1, error);
                }

                if (options.InlineParameters != null && options.InlineParameters.Any())
                {
                    parser.ParseParametersAndMaterializeFunctions(out parameters, _cancellationTokenSource.Token);
                }
                else
                {
                    parser.MaterializeFunctionsInsideTemplate(_cancellationTokenSource.Token);
                }
            }

            var whatIfParser = new WhatIfParser(templateType, scopeId, resourceGroupName, template, parser?.Parameters, logger, commandType, location, options);
            var whatIfData = await whatIfParser.GetWhatIfData(_cancellationTokenSource.Token);
            if (whatIfData == null)
            {
                var error = "Couldn't fetch data for What If request.";
                logger.LogError("{error}", error);
                
                return new ApplicationResult(1, error);
            }

            if (whatIfData != null && whatIfData.status == "Failed")
            {
                var error = $"An error happened when performing WhatIf operation with status {whatIfData.status}.";
                logger.LogError("{error}", error);

                if (whatIfData.error != null)
                {
                    var errorDetails = JsonSerializer.Serialize(whatIfData.error, typeof(WhatIfError), new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    });

                    logger.LogError("{error}", errorDetails);
                }

                return new ApplicationResult(1, error);
            }

            if (whatIfData == null || whatIfData.properties == null || whatIfData.properties.changes == null || whatIfData.properties.changes.Length == 0)
            {
                logger.AddEstimatorMessage("No changes detected.");
                return new ApplicationResult(0, null);
            }

            logger.AddEstimatorMessage("Detected {0} resources.", whatIfData.properties.changes.Length);
            logger.LogInformation("");

            ReportChangesToConsole(whatIfData.properties.changes, logger);

            logger.LogInformation("");
            logger.LogInformation("-------------------------------");
            logger.LogInformation("");

            if (options.DryRunOnly)
            {
                logger.LogInformation("Dry run enabled, skipping estimation.");
                return new ApplicationResult(0, null);;
            }

            using var processor =  new WhatIfProcessor(logger,
                                                   whatIfData.properties.changes,
                                                   parser?.Template!,
                                                   options,
                                                   _cancellationTokenSource.Token);

            var output = await processor.ProcessAsync(_cancellationTokenSource.Token);

            await GenerateOutputIfNeeded(options, output, logger);

            if (options.Threshold != -1 && output.TotalCost.OriginalValue > options.Threshold)
            {
                var error = $"Estimated cost [{output.TotalCost} USD] exceeds configured threshold [{options.Threshold} USD].";
                logger.LogError("{error}", error);
                
                return new ApplicationResult(1, error);
            }

            return new ApplicationResult(0, null);
        }
    }

    private static string? GetTemplate(FileInfo templateFile, string? terraformExecutable, bool forceUsingBicepCli, ILogger<Program> logger, out TemplateType templateType)
    {
        var compiler = new TemplateCompiler(templateFile, terraformExecutable, forceUsingBicepCli, logger);
        templateType = compiler.TemplateType;

        return compiler.Compile(_cancellationTokenSource.Token);
    }

    private static async Task GenerateOutputIfNeeded(EstimateOptions options, EstimationOutput output, ILogger<Program> logger)
    {
        var handler = new OutputHandler(logger);
        await handler.GenerateOutputIfNeeded(options, output);
    }

    private static async Task DisplayWelcomeScreenAsync(ILogger<Program> logger, bool isNewVersionCheckDisabled)
    {
        logger.LogInformation(@"                                                                                                   
                                              .***      ***                                         
                                   ,    ***.********,****                                           
                                     **********************                                         
                               *    ************. ***********  *                                    
                             ///   *******, ****  ////////////******                                
                               .//************ ///// /////////********                              
                           ///////// ********///////// //////.********    (,                        
                     .   ///////////// **** ////////////,////******* ( (/                           
                       *////////////////   ////////////////  ** ((((((((((((                        
                        ///,  */////////.   **************,    ((((((((((((((                       
                       ///////////////.(((((((,******** ///// (((((( (((((((((                      
                       ///////////// ((((((((((((   ////////// ((((((((((*((((                      
                        ////////// ((((((((((( ((    //////////(((((((((((((((                      
                    //   /////// (((((( (((((((((((((((( ///////(((((((((((((                       
                       //   /  ( ((((((((((((((((,((((((((((.////(((((((((  ((                      
                                                                                                    
                                  ACE (Azure Cost Estimator) [{version}]", GetInformationalVersion());
        logger.LogInformation("");
        logger.LogInformation("General help: https://github.com/TheCloudTheory/arm-estimator/discussions");
        logger.LogInformation("Bugs & issues: https://github.com/TheCloudTheory/arm-estimator/issues");
        logger.LogInformation("");
        logger.LogInformation("------------------------------");
        logger.LogInformation("");

        var check = new NewestVersionCheck(logger);
        await check.DisplayCheckInfoAsync(GetInformationalVersion()!, isNewVersionCheckDisabled);
    }

    private static void DisplayUsedSettings(FileInfo templateFile, string scopeId, string? resourceGroupName, ILogger<Program> logger, EstimateOptions options, CommandType commandType)
    {
        logger.LogInformation("Run configuration:");
        logger.LogInformation("");
        logger.AddEstimatorMessage("Scope: {0}", commandType.ToString());

        switch (commandType)
        {
            case CommandType.ResourceGroup:
                logger.AddEstimatorMessage("Susbcription Id: {0}", scopeId);
                logger.AddEstimatorMessage("Resource group: {0}", resourceGroupName);
                break;
            case CommandType.Subscription:
                logger.AddEstimatorMessage("Susbcription Id: {0}", scopeId);
                break;
            case CommandType.ManagementGroup:
                logger.AddEstimatorMessage("Management Group Id: {0}", scopeId);
                break;
            case CommandType.Tenant:
                logger.AddEstimatorMessage("Tenant Id: {0}", scopeId);
                break;
        }

        logger.AddEstimatorMessage("Template file: {0}", templateFile);
        logger.AddEstimatorMessage("Deployment mode: {0}", options.Mode);
        logger.AddEstimatorMessage("Threshold: {0}", options.Threshold == -1 ? "Not Set" : options.Threshold.ToString());
        logger.AddEstimatorMessage("Parameters file: {0}", options.ParametersFile?.Name ?? "Not Set");
        logger.AddEstimatorMessage("Currency: {0}", options.Currency);
        logger.AddEstimatorMessage("Generate JSON output: {0}", options.ShouldGenerateJsonOutput);
        logger.AddEstimatorMessage("Silent mode: {0}", options.ShouldBeSilent);
        logger.AddEstimatorMessage("Redirect stdout: {0}", options.Stdout);
        logger.AddEstimatorMessage("Disabled detailed metrics: {0}", options.DisableDetailedMetrics);
        logger.AddEstimatorMessage("Generate HTML output: {0}", options.ShouldGenerateHtmlOutput);
        logger.AddEstimatorMessage("HTML output filename: {0}", string.IsNullOrEmpty(options.HtmlOutputFilename) ? "Not Set" : options.HtmlOutputFilename + ".html");
        logger.AddEstimatorMessage("Dry run enabled: {0}", options.DryRunOnly);
        logger.AddEstimatorMessage("Cache disabled: {0}", options.DisableCache);
        logger.AddEstimatorMessage("Cache handler: {0}", options.DisableCache ? "Disabled" : options.CacheHandler.ToString());
        logger.AddEstimatorMessage("Conversion rate: {0}", options.ConversionRate);
        logger.AddEstimatorMessage("Webhook URL: {0}", string.IsNullOrEmpty(options.WebhookUrl) ? "Disabled" : options.WebhookUrl);
        logger.AddEstimatorMessage("Log file: {0}", string.IsNullOrEmpty(options.LogFile) ? "Disabled" : options.LogFile);
        logger.AddEstimatorMessage("Disable version check: {0}", options.OptOutCheckingNewVersion ? "True" : "False");
        logger.AddEstimatorMessage("Generated What If file:", string.IsNullOrEmpty(options.UserGeneratedWhatIfFile) ? "Disabled" : options.UserGeneratedWhatIfFile);
        logger.AddEstimatorMessage("Generate Markdown output: {0}", options.ShouldGenerateMarkdownOutput);
        logger.AddEstimatorMessage("Markdown output filename: {0}", string.IsNullOrEmpty(options.MarkdownOutputFilename) ? "Not Set" : options.MarkdownOutputFilename);
        logger.LogInformation("");
        logger.LogInformation("------------------------------");
        logger.LogInformation("");
    }

    private static void ReportChangesToConsole(WhatIfChange[] changes, ILogger logger)
    {
        foreach (var change in changes)
        {
            if (change == null) continue;

            if (change.resourceId == null)
            {
                logger.LogWarning("Couldn't find resource ID.");
                continue;
            }

            if (change.changeType == null)
            {
                logger.LogWarning("Couldn't find change type.");
                continue;
            }

            var id = new CommonResourceIdentifier(change.resourceId);
            logger.AddEstimatorMessageSensibleToChange(change.changeType, "{0} [{1}]", id.GetName(), id.GetResourceType());
        }
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespacesRegex();
}