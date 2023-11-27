using ACE.Compilation;
using ACE.Output;
using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ACE;

public class Program
{
    private static string? GetInformationalVersion() => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    private static readonly CancellationTokenSource _cancellationTokenSource = new();

    public static async Task<int> Main(string[] args)
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

        var deploymentModeOption = new Option<DeploymentMode>("--mode", () => { return DeploymentMode.Incremental; }, "Deployment mode");
        var thresholdOption = new Option<int?>("--threshold", () => { return null; }, "Estimation threshold");
        var parametersOption = new Option<FileInfo?>("--parameters", () => { return null; }, "Path to a file containing values of template parameters");
        var currencyOption = new Option<CurrencyCode>("--currency", () => { return CurrencyCode.USD; }, "Currency code");
        var jsonOutputOption = new Option<bool>("--generateJsonOutput", () => { return false; }, "Should generate JSON output");
        var silentOption = new Option<bool>("--silent", () => { return false; }, "Mute all logs");
        var stdoutOption = new Option<bool>("--stdout", () => { return false; }, "Redirects JSON output to stdout");
        var disableDetailsOption = new Option<bool>("--disableDetailedMetrics", () => { return false; }, "Disables reporting of detailed metrics");
        var jsonOutputFilenameOption = new Option<string?>("--jsonOutputFilename", () => { return null; }, "Sets JSON output filename");
        var htmlOutputOption = new Option<bool>("--generateHtmlOutput", () => { return false; }, "Should generate HTML output");
        var inlineOptions = new Option<IEnumerable<string>>("--inline", () => { return Enumerable.Empty<string>(); }, "List of inline parameters");
        var dryRunOption = new Option<bool>("--dry-run", () => { return false; }, "Run template validation only");
        var htmlOutputFilenameOption = new Option<string?>("--htmlOutputFilename", () => { return null; }, "Sets HTML output filename");
        var outputFormatOption = new Option<OutputFormat>("--outputFormat", () => { return OutputFormat.Default; }, "Sets output format");
        var disableCacheOption = new Option<bool>("--disable-cache", () => false, "Disables in-built cache mechanism");
        var terraformExecutableOption = new Option<string?>("--tf-executable", () => null, "Provide path to Terraform executable. If omitted, ACE will try to find it in PATH");
        var conversionRateOption = new Option<double>("--conversion-rate", () => 1.0, "Conversion rate from USD to selected currency.");
        var cacheHandlerOption = new Option<CacheHandler>("--cache-handler", () => { return CacheHandler.Local; }, "Selected cache handler to be used to store cached data");
        var cacheStorageAccountNameOption = new Option<string?>("--cache-storage-account-name", () => { return null; }, "Name of Azure Storage account to be used as cache storage. Required, if cache handler is set to AzureStorage");
        var webhookUrlOption = new Option<string?>("--webhook-url", () => { return null; }, "Webhook URL to be used for sending estimation result");
        var webhookAuthorizationOption = new Option<string?>("--webhook-authorization", () => { return null; }, "Webhook Authorization header value");
        var logFileOption = new Option<string?>("--log-file", () => { return null; }, "Path to a log file");
        var configurationFileOption = new Option<FileInfo?>("--configuration-file", () => { return null; }, "Path to configuration file for ACE");

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

        rootCommand.AddArgument(templateFileArg);
        rootCommand.AddArgument(susbcriptionIdArg);
        rootCommand.AddArgument(resourceGroupArg);

        rootCommand.SetHandler(async (file, subscription, resourceGroup, options) =>
        {
            var exitCode = await Estimate(file, subscription, resourceGroup, null, options, CommandType.ResourceGroup);
            if (exitCode != 0)
            {
                throw new Exception();
            }
        },
            templateFileArg,
            susbcriptionIdArg,
            resourceGroupArg,
            new EstimateOptionsBinder(
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
                configurationFileOption
        ));

        var subscriptionCommand = new Command("sub", "Calculate estimation for subscription");
        subscriptionCommand.AddArgument(templateFileArg);
        subscriptionCommand.AddArgument(susbcriptionIdArg);
        subscriptionCommand.AddArgument(locationArg);

        subscriptionCommand.SetHandler(async (file, subscription, location, options) =>
        {
            var exitCode = await Estimate(file, subscription, null, location, options, CommandType.Subscription);
            if (exitCode != 0)
            {
                throw new Exception();
            }
        },
            templateFileArg,
            susbcriptionIdArg,
            locationArg,
            new EstimateOptionsBinder(
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
                configurationFileOption
        ));

        var managementGroupCommand = new Command("mg", "Calculate estimation for management group");
        managementGroupCommand.AddArgument(templateFileArg);
        managementGroupCommand.AddArgument(managementGroupArg);
        managementGroupCommand.AddArgument(locationArg);

        managementGroupCommand.SetHandler(async (file, managementGroup, location, options) =>
        {
            var exitCode = await Estimate(file, managementGroup, null, location, options, CommandType.ManagementGroup);
            if (exitCode != 0)
            {
                throw new Exception();
            }
        },
            templateFileArg,
            managementGroupArg,
            locationArg,
            new EstimateOptionsBinder(
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
                configurationFileOption
        ));

        var tenantCommand = new Command("tenant", "Calculate estimation for tenant");
        tenantCommand.AddArgument(templateFileArg);
        tenantCommand.AddArgument(locationArg);

        tenantCommand.SetHandler(async (file, location, options) =>
        {
            var exitCode = await Estimate(file, "<tenant>", null, location, options, CommandType.Tenant);
            if (exitCode != 0)
            {
                throw new Exception();
            }
        },
            templateFileArg,
            locationArg,
            new EstimateOptionsBinder(
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
                configurationFileOption
        ));

        rootCommand.AddCommand(subscriptionCommand);
        rootCommand.AddCommand(managementGroupCommand);
        rootCommand.AddCommand(tenantCommand);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task<int> Estimate(FileInfo templateFile,
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

            DisplayWelcomeScreen(logger);
            DisplayUsedSettings(templateFile, scopeId, resourceGroupName, logger, options, commandType);

            var template = GetTemplate(templateFile, options.TerraformExecutable, logger, out var templateType);
            if (template == null)
            {
                logger.LogError("There was a problem with processing template.");
                return 1;
            }

            var parameters = "{}";
            if (options.ParametersFile != null)
            {
                parameters = Regex.Replace(File.ReadAllText(options.ParametersFile.FullName), @"\s+", string.Empty);
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
                    logger.LogError("Couldn't parse the following template - {template}. Error: {error}", template, ex.Message);
                    return 1;
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

            var whatIfParser = new WhatIfParser(templateType, scopeId, resourceGroupName, template, parameters, logger, commandType, location, options);
            var whatIfData = await whatIfParser.GetWhatIfData(_cancellationTokenSource.Token);
            if (whatIfData == null)
            {
                logger.LogError("Couldn't fetch data for What If request.");
                return 1;
            }

            if (whatIfData != null && whatIfData.status == "Failed")
            {
                logger.LogError("An error happened when performing WhatIf operation.");

                if (whatIfData.error != null)
                {
                    var errorDetails = JsonSerializer.Serialize(whatIfData.error, typeof(WhatIfError), new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    });

                    logger.LogError("{error}", errorDetails);
                }

                return 1;
            }

            if (whatIfData == null || whatIfData.properties == null || whatIfData.properties.changes == null || whatIfData.properties.changes.Length == 0)
            {
                logger.AddEstimatorMessage("No changes detected.");
                return 0;
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
                return 0;
            }

            var output = await new WhatIfProcessor(logger,
                                                   whatIfData.properties.changes,
                                                   parser?.Template,
                                                   options,
                                                   _cancellationTokenSource.Token).Process(_cancellationTokenSource.Token);
            await GenerateOutputIfNeeded(options, output, logger);

            if (options.Threshold != -1 && output.TotalCost.OriginalValue > options.Threshold)
            {
                logger.LogError("Estimated cost [{totalCost} USD] exceeds configured threshold [{threshold} USD].", output.TotalCost, options.Threshold);
                return 1;
            }

            return 0;
        }
    }

    private static string? GetTemplate(FileInfo templateFile, string? terraformExecutable, ILogger<Program> logger, out TemplateType templateType)
    {
        var compiler = new TemplateCompiler(templateFile, terraformExecutable, logger);
        templateType = compiler.TemplateType;

        return compiler.Compile(_cancellationTokenSource.Token);
    }

    private static async Task GenerateOutputIfNeeded(EstimateOptions options, EstimationOutput output, ILogger<Program> logger)
    {
        if (options.ShouldGenerateJsonOutput || options.ShouldGenerateHtmlOutput)
        {
            if (options.ShouldGenerateJsonOutput)
            {
                var outputData = JsonSerializer.Serialize(output);
                if (options.Stdout)
                {
                    logger.AddEstimatorNonSilentMessage(outputData);
                }
                else
                {
                    var fileName = options.JsonOutputFilename != null ? $"{options.JsonOutputFilename}.json" : $"ace_estimation_{DateTime.UtcNow:yyyyMMddHHmmss}.json";
                    logger.AddEstimatorMessage("Generating JSON output file as {0}", fileName);
                    File.WriteAllText(fileName, outputData);
                }
            }

            if (options.ShouldGenerateHtmlOutput)
            {
                var generator = new HtmlOutputGenerator(output, logger, options.HtmlOutputFilename);
                generator.Generate();
            }

            if(!string.IsNullOrEmpty(options.WebhookUrl))
            {
                logger.AddEstimatorMessage("Sending estimation result to webhook URL {0}", options.WebhookUrl);

                var client = new HttpClient();
                var message = new HttpRequestMessage(HttpMethod.Post, options.WebhookUrl)
                {
                    Content = new StringContent(JsonSerializer.Serialize(output), Encoding.UTF8, "application/json")
                };

                if(string.IsNullOrEmpty(options.WebhookAuthorization))
                {
                    logger.AddEstimatorMessage("Webhook authorization header not set, skipping.");
                }
                else
                {
                    message.Headers.Add("Authorization", options.WebhookAuthorization);
                }

                var response = await client.SendAsync(message);
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError("Couldn't send estimation result to webhook URL {url}. Status code: {code}", options.WebhookUrl, response.StatusCode);
                }
                else 
                {
                    logger.AddEstimatorMessage("Estimation result sent successfully to webhook URL {0}", options.WebhookUrl);
                }
            }
        }

        return;
    }

    private static void DisplayWelcomeScreen(ILogger<Program> logger)
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
}