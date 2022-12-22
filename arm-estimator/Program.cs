using Azure.Core;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

public class Program
{
    private static string? GetInformationalVersion() => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

    public static async Task<int> Main(string[] args)
    {
        var templateFileArg = new Argument<FileInfo>("template-file", "Template file to analyze");
        var susbcriptionIdArg = new Argument<string>("subscription-id", "Subscription ID");
        var resourceGroupArg = new Argument<string>("resource-group", "Resource group name");
        var deploymentModeOption = new Option<DeploymentMode>("--mode", () => { return DeploymentMode.Incremental; }, "Deployment mode");
        var thresholdOption = new Option<int>("--threshold", () => { return -1; }, "Estimation threshold");
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

        var command = new RootCommand("ACE (Azure Cost Estimator)")
        {
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
            dryRunOption
        };

        command.AddArgument(templateFileArg);
        command.AddArgument(susbcriptionIdArg);
        command.AddArgument(resourceGroupArg);
        command.SetHandler(async (file, subscription, resourceGroup, options) =>
        {
            await Estimate(file, subscription, resourceGroup, options);
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
                dryRunOption
            ));

        return await command.InvokeAsync(args);
    }

    private static async Task Estimate(FileInfo templateFile, string subscriptionId, string resourceGroupName, EstimateOptions options)
    {
        using (var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddEstimatorLogger(options.ShouldBeSilent);
        }))
        {
            var logger = loggerFactory.CreateLogger<Program>();

            DisplayWelcomeScreen(logger);
            DisplayUsedSettings(templateFile, subscriptionId, resourceGroupName, logger, options);

            await Task.CompletedTask;
            // var template = GetTemplate(templateFile, logger);
            // if (template == null)
            // {
            //     logger.LogError("There was a problem with processing template.");
            //     return;
            // }

            // var parameters = "{}";
            // if (options.ParametersFile != null)
            // {
            //     parameters = Regex.Replace(File.ReadAllText(options.ParametersFile.FullName), @"\s+", string.Empty);
            // }

            // var parser = new TemplateParser(template, parameters, options.InlineParameters, logger);
            // if (options.InlineParameters.Any())
            // {
            //     parser.ParseInlineParameters(out parameters);
            // }

            // var handler = new AzureWhatIfHandler(subscriptionId, resourceGroupName, template, options.Mode, parameters, logger);
            // var whatIfData = await handler.GetResponseWithRetries();
            // if (whatIfData == null)
            // {
            //     Environment.Exit(1);
            // }

            // if (whatIfData != null && whatIfData.status == "Failed")
            // {
            //     logger.LogError("An error happened when performing WhatIf operation.");

            //     if (whatIfData.error != null)
            //     {
            //         var errorDetails = JsonSerializer.Serialize(whatIfData.error, typeof(WhatIfError), new JsonSerializerOptions()
            //         {
            //             WriteIndented = true
            //         });

            //         logger.LogError("{error}", errorDetails);
            //     }

            //     return;
            // }

            // if (whatIfData == null || whatIfData.properties == null || whatIfData.properties.changes == null || whatIfData.properties.changes.Length == 0)
            // {
            //     logger.AddEstimatorMessage("No changes detected.");
            //     return;
            // }

            // logger.AddEstimatorMessage("Detected {0} resources.", whatIfData.properties.changes.Length);
            // logger.LogInformation("");

            // ReportChangesToConsole(whatIfData.properties.changes, logger);

            // logger.LogInformation("");
            // logger.LogInformation("-------------------------------");
            // logger.LogInformation("");

            // if (options.DryRunOnly)
            // {
            //     logger.LogInformation("Dry run enabled, skipping estimation.");
            //     return;
            // }

            // var output = await new WhatIfProcessor(logger, whatIfData.properties.changes, options.Currency, options.DisableDetailedMetrics, parser.Template).Process();
            // GenerateOutputIfNeeded(options, output, logger);

            // if (options.Threshold != -1 && output.TotalCost > options.Threshold)
            // {
            //     logger.LogError("Estimated cost [{totalCost} USD] exceeds configured threshold [{threshold} USD].", output.TotalCost, options.Threshold);
            //     Environment.Exit(1);
            // }
        }
    }

    private static string? GetTemplate(FileInfo templateFile, ILogger<Program> logger)
    {
        string? template = null;
        if (templateFile.Extension == ".bicep")
        {
            try
            {
                logger.AddEstimatorMessage("Attempting to compile Bicep file using Bicep CLI.");
                CompileBicepWith("bicep", $"build {templateFile} --stdout", logger, out template);
            }
            catch (Win32Exception)
            {
                // First compilation may not work if Bicep CLI is not installed directly,
                // try to use Azure CLI instead
                logger.AddEstimatorMessage("Compilation failed, attempting to compile Bicep file using Azure CLI.");
                CompileBicepWith("az", $"bicep build --file {templateFile} --stdout", logger, out template);
            }
        }
        else
        {
            template = Regex.Replace(File.ReadAllText(templateFile.FullName), @"\s+", string.Empty);  // Make JSON a single-line value
        }

        return template;
    }

    private static void CompileBicepWith(string fileName, string arguments, ILogger logger, out string? template)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = fileName;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            string? error = null;
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });

            process.Start();
            process.BeginErrorReadLine();
            template = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrWhiteSpace(template))
            {
                logger.LogError("{error}", error);
                template = null;

                return;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(error) == false)
                {
                    // Bicep returns warnings as errors, so if a template is generated,
                    // that most likely the case and we need to handle it
                    logger.LogWarning("{warning}", error);
                }
            }

            logger.AddEstimatorMessage("Compilation completed!");
            logger.LogInformation("");
            logger.LogInformation("------------------------------");
            logger.LogInformation("");
        }
    }

    private static void GenerateOutputIfNeeded(EstimateOptions options, EstimationOutput output, ILogger<Program> logger)
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
                var generator = new HtmlOutputGenerator(output, logger);
                generator.Generate();
            }
        }
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

    private static void DisplayUsedSettings(FileInfo templateFile, string subscriptionId, string resourceGroupName, ILogger<Program> logger, EstimateOptions options)
    {
        logger.LogInformation("Run configuration:");
        logger.LogInformation("");
        logger.AddEstimatorMessage("SubscriptionId: {0}", subscriptionId);
        logger.AddEstimatorMessage("Resource group: {0}", resourceGroupName);
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
        logger.AddEstimatorMessage("Dry run enabled: {0}", options.DryRunOnly);
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

            var id = new ResourceIdentifier(change.resourceId);
            logger.AddEstimatorMessageSensibleToChange(change.changeType, "{0} [{1}]", id.Name, id.ResourceType);
        }
    }
}