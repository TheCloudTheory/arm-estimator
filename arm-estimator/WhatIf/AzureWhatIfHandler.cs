using Azure.Core;
using Azure.Identity;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

internal class AzureWhatIfHandler
{
    private static readonly Lazy<HttpClient> httpClient = new(() => new HttpClient());

    public static async Task<WhatIfResponse?> GetResponseWithRetries(string subscriptionId, string resourceGroupName, string template)
    {
        var response = await SendInitialRequest(subscriptionId, resourceGroupName, template);

        // API can return HTTP 202 if asynchronous processing is needed.
        // If so, we need to check Location header to get final information
        // TODO: Respect 'Retry-after' header
        if (response.StatusCode == HttpStatusCode.Accepted)
        {
            var location = response.Headers.Location;
            if(location == null)
            {
                throw new Exception("Location header can't be null when awaiting response.");
            }

            response = await SendAndWaitForResponse(subscriptionId, resourceGroupName, template, location);
        }

#if DEBUG
        var rawData = await response.Content.ReadAsStringAsync();
#endif

        var data = JsonSerializer.Deserialize<WhatIfResponse>(await response.Content.ReadAsStreamAsync());
        return data;
    }

    private static async Task<HttpResponseMessage> SendInitialRequest(string subscriptionId, string resourceGroupName, string template)
    {
        var token = GetToken();
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://management.azure.com/subscriptions/{subscriptionId}/resourcegroups/{resourceGroupName}/providers/Microsoft.Resources/deployments/arm-estimator/whatIf?api-version=2021-04-01");
        var templateContent = JsonSerializer.Serialize(new EstimatePayload(template), new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        request.Content = new StringContent(templateContent, Encoding.UTF8, "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.Value.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return response;
    }

    private static async Task<HttpResponseMessage> SendAndWaitForResponse(string subscriptionId, string resourceGroupName, string template, Uri location)
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
