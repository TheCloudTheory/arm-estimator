using System.CommandLine;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using Azure.Core;
using Azure.Identity;

internal class Program
{
    private static readonly Lazy<HttpClient> httpClient = new Lazy<HttpClient>(() => new HttpClient());

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
        Console.WriteLine($"Processing file {file.FullName}.");

        var template = Regex.Replace(File.ReadAllText(file.FullName), @"\s+", string.Empty);
        var options = new DefaultAzureCredentialOptions() 
        {
            ExcludeVisualStudioCodeCredential = true,
            ExcludeVisualStudioCredential = true
        };
        var token = new DefaultAzureCredential(options).GetToken(new TokenRequestContext(new [] {"https://management.azure.com/.default" }), CancellationToken.None).Token;
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups/{resourceGroupName}/providers/Microsoft.Resources/deployments/arm-estimator/whatIf?api-version=2021-04-01");
        var templateContent = JsonSerializer.Serialize(new EstimatePayload(template), new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        request.Content = new StringContent(templateContent, Encoding.UTF8, "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.Value.SendAsync(request);
        var data = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        // API can return HTTP 202 if asynchronous processing is needed.
        // If so, we need to check Location header to get final information
        // TODO: Respect 'Retry-after' header
        if(response.StatusCode == HttpStatusCode.Accepted)
        {
            var location = response.Headers.Location;
            var asynchronousOperationRequest = new HttpRequestMessage(HttpMethod.Get, location);

            asynchronousOperationRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var asynchronousOperationResponse = await httpClient.Value.SendAsync(asynchronousOperationRequest);
            var asynchronousOperationData = await asynchronousOperationResponse.Content.ReadAsStringAsync();

            asynchronousOperationResponse.EnsureSuccessStatusCode();
        }
    }
}