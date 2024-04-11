using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ACE.Output
{
    /// <summary>
    /// Handles the generation and sending of output based on the provided options and estimation output.
    /// </summary>
    internal class OutputHandler
    {
        private readonly ILogger<Program> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputHandler"/> class with the specified logger.
        /// </summary>
        /// <param name="logger">The logger to use for logging messages.</param>
        public OutputHandler(ILogger<Program> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Generates the output if needed based on the provided options and estimation output.
        /// </summary>
        /// <param name="options">The estimation options.</param>
        /// <param name="output">The estimation output.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GenerateOutputIfNeeded(EstimateOptions options, EstimationOutput output)
        {
            if (options.ShouldGenerateJsonOutput || options.ShouldGenerateHtmlOutput || options.ShouldGenerateMarkdownOutput)
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
                        var fileName = GenerateJsonOutputFilename(options);
                        logger.AddEstimatorMessage("Generating JSON output file as {0}", fileName);
                        File.WriteAllText(fileName, outputData);
                    }
                }

                if (options.ShouldGenerateHtmlOutput)
                {
                    var generator = new HtmlOutputGenerator(output, logger, options.HtmlOutputFilename);
                    generator.Generate();
                }

                if (options.ShouldGenerateMarkdownOutput)
                {
                    var generator = new MarkdownOutputGenerator(output, logger, options.MarkdownOutputFilename);
                    generator.Generate();
                }

                if (!string.IsNullOrEmpty(options.WebhookUrl))
                {
                    logger.AddEstimatorMessage("Sending estimation result to webhook URL {0}", options.WebhookUrl);

                    var client = new HttpClient();
                    var message = new HttpRequestMessage(HttpMethod.Post, options.WebhookUrl)
                    {
                        Content = new StringContent(JsonSerializer.Serialize(output), Encoding.UTF8, "application/json")
                    };

                    if (string.IsNullOrEmpty(options.WebhookAuthorization))
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

        private static string GenerateJsonOutputFilename(EstimateOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.JsonOutputFilename))
            {
                return $"ace_estimation_{DateTime.UtcNow:yyyyMMddHHmmss}.json";
            }

            if (options.JsonOutputFilename.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return options.JsonOutputFilename;
            }

            return $"{options.JsonOutputFilename}.json";
        }
    }
}
