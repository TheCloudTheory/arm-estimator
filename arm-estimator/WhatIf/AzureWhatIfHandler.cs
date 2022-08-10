using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

internal class AzureWhatIfHandler
{
    private static readonly Lazy<HttpClient> httpClient = new(() => new HttpClient());

    private readonly string subscriptionId;
    private readonly string resourceGroupName;
    private readonly string template;
    private readonly DeploymentMode deploymentMode;
    private readonly string parameters;
    private readonly ILogger logger;

    public AzureWhatIfHandler(string subscriptionId,
                              string resourceGroupName,
                              string template,
                              DeploymentMode deploymentMode,
                              string parameters,
                              ILogger logger)
    {
        this.subscriptionId = subscriptionId;
        this.resourceGroupName = resourceGroupName;
        this.template = template;
        this.deploymentMode = deploymentMode;
        this.parameters = parameters;
        this.logger = logger;
    }

    public async Task<WhatIfResponse?> GetResponseWithRetries()
    {
        this.logger.LogInformation("What If operation will be performed using '{mode}' deployment mode.", this.deploymentMode);
        
        var response = await SendInitialRequest();
        var maxRetries = 5;
        var currentRetry = 1;
        
        while (response.StatusCode == HttpStatusCode.Accepted && currentRetry < maxRetries)
        {
            var retryAfterHeader = response.Headers.RetryAfter;
            var retryAfter = retryAfterHeader == null ? TimeSpan.FromSeconds(15) : retryAfterHeader.Delta;

            this.logger.LogInformation("Waiting for response from What If API.");
            await Task.Delay(retryAfter.HasValue ? retryAfter.Value.Seconds * 1000 : 15000);

            var location = response.Headers.Location;

            if(location == null)
            {
                throw new Exception("Location header can't be null when awaiting response.");
            }

            response = await SendAndWaitForResponse(location);
            currentRetry++;
        }

#if DEBUG
        var rawData = await response.Content.ReadAsStringAsync();
#endif

        var data = JsonSerializer.Deserialize<WhatIfResponse>(await response.Content.ReadAsStreamAsync());
        return data;
    }

    private async Task<HttpResponseMessage> SendInitialRequest()
    {
        var token = GetToken();
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://management.azure.com/subscriptions/{this.subscriptionId}/resourcegroups/{this.resourceGroupName}/providers/Microsoft.Resources/deployments/arm-estimator/whatIf?api-version=2021-04-01");
        var templateContent = JsonSerializer.Serialize(new EstimatePayload(this.template, this.deploymentMode, this.parameters), new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        request.Content = new StringContent(templateContent, Encoding.UTF8, "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.Value.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return response;
    }

    private static async Task<HttpResponseMessage> SendAndWaitForResponse(Uri location)
    {
        var token = GetToken();
        var request = new HttpRequestMessage(HttpMethod.Get, location);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.Value.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return response;
    }

    private static string GetToken()
    {
        var options = new DefaultAzureCredentialOptions()
        {
            ExcludeVisualStudioCodeCredential = true,
            ExcludeVisualStudioCredential = true
        };

        var token = new DefaultAzureCredential(options).GetToken(new TokenRequestContext(new[] { "https://management.azure.com/.default" }), CancellationToken.None).Token;
        return token;
    }
}
