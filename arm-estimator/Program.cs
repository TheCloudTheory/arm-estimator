using Azure.Core;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

internal class Program
{
    public static string? GetInformationalVersion() => Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

    private static async Task<int> Main(string[] args)
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

        var command = new RootCommand("Azure Resource Manager cost estimator.")
        {
            deploymentModeOption,
            thresholdOption,
            parametersOption,
            currencyOption,
            jsonOutputOption,
            silentOption
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
            silentOption));

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

            var template = Regex.Replace(File.ReadAllText(templateFile.FullName), @"\s+", string.Empty);  // Make JSON a single-line value
            var parameters = "{}";

            if (options.ParametersFile != null)
            {
                parameters = Regex.Replace(File.ReadAllText(options.ParametersFile.FullName), @"\s+", string.Empty);
            }

            var handler = new AzureWhatIfHandler(subscriptionId, resourceGroupName, template, options.Mode, parameters, logger);
            var whatIfData = await handler.GetResponseWithRetries();

            if (whatIfData != null && whatIfData.status == "Failed")
            {
                logger.LogError("An error happened when performing WhatIf operation.");

                if (whatIfData.error != null)
                {
                    var errorDetails = JsonSerializer.Serialize(whatIfData.error, typeof(WhatIfError), new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    });

                    logger.LogError(errorDetails);
                }

                return;
            }

            if (whatIfData == null || whatIfData.properties == null || whatIfData.properties.changes == null || whatIfData.properties.changes.Length == 0)
            {
                logger.AddEstimatorMessage("No changes detected.");
                return;
            }

            logger.AddEstimatorMessage("Detected {0} resources.", whatIfData.properties.changes.Length);
            logger.LogInformation("");

            ReportChangesToConsole(whatIfData.properties.changes, logger);

            logger.LogInformation("");
            logger.LogInformation("-------------------------------");
            logger.LogInformation("");

            var output = await new WhatIfProcessor(logger, whatIfData.properties.changes, options.Currency).Process();

            if(options.ShouldGenerateOutput)
            {
                var outputData = JsonSerializer.Serialize(output);
                File.WriteAllText($"ace_estimation_{DateTime.UtcNow:yyyyMMddHHmmss}.json", outputData);
            }

            if (options.Threshold != -1 && output.TotalCost > options.Threshold)
            {
                logger.LogError("Estimated cost [{totalCost} USD] exceeds configured threshold [{threshold} USD].", output.TotalCost, options.Threshold);
                Environment.Exit(1);
            }
        }
    }

    private static void DisplayWelcomeScreen(ILogger<Program> logger)
    {
        logger.LogInformation("ARM Cost Estimator [{version}]", GetInformationalVersion());
        logger.LogInformation("------------------------------");
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
        logger.AddEstimatorMessage("Generate JSON output?: {0}", options.ShouldGenerateOutput);
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