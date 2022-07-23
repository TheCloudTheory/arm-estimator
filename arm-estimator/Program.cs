using Azure.Core;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.Text.RegularExpressions;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var templateFileArg = new Argument<FileInfo>("template-file", "Template file to analyze");
        var susbcriptionIdArg = new Argument<string>("subscription-id", "Susbcription ID");
        var resourceGroupArg = new Argument<string>("resource-group", "Resource group name");

        var command = new RootCommand("Azure Resource Manager cost estimator.");
        command.AddArgument(templateFileArg);
        command.AddArgument(susbcriptionIdArg);
        command.AddArgument(resourceGroupArg);
        command.SetHandler(async (file, subscription, resourceGroup) =>
            await Estimate(file, subscription, resourceGroup), templateFileArg, susbcriptionIdArg, resourceGroupArg);

        return await command.InvokeAsync(args);
    }

    private static async Task Estimate(FileInfo file, string subscriptionId, string resourceGroupName)
    {
        using (var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddEstimatorLogger();
        }))
        {
            var logger = loggerFactory.CreateLogger<Program>();
            var template = Regex.Replace(File.ReadAllText(file.FullName), @"\s+", string.Empty);  // Make JSON a single-line value
            var whatIfData = await AzureWhatIfHandler.GetResponseWithRetries(subscriptionId, resourceGroupName, template);

            if (whatIfData == null || whatIfData.properties == null || whatIfData.properties.changes == null || whatIfData.properties.changes.Length == 0)
            {
                logger.LogInformation("No changes detected.");
                return;
            }

            logger.LogInformation("Detected {noOfChanges} changes.", whatIfData.properties.changes.Length);
            logger.LogInformation("-------------------------------");

            ReportChangesToConsole(whatIfData.properties.changes, logger);

            logger.LogInformation("");
            logger.LogInformation("-------------------------------");
            logger.LogInformation("");

            await new WhatIfProcessor(logger).Process(whatIfData.properties.changes);
        }
    }

    private static void ReportChangesToConsole(WhatIfChange[] changes, ILogger logger)
    {
        foreach (var change in changes)
        {
            if (change == null) continue;

            if(change.resourceId == null)
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
            var formattedChangeType = change.changeType?.ToString().ToUpperInvariant();
            logger.LogInformation("{change} - {name} [{type}]", formattedChangeType, id.Name, id.ResourceType);
        }
    }
}