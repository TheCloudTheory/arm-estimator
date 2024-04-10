using ACE.Caching;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ACE.WhatIf;

internal class AzureWhatIfHandler
{
    private static readonly Lazy<HttpClient> httpClient = new(() => new HttpClient());

    private readonly string scopeId;
    private readonly string? resourceGroupName;
    private readonly string template;
    private readonly DeploymentMode deploymentMode;
    private readonly string parameters;
    private readonly ILogger logger;
    private readonly CommandType commandType;
    private readonly string? location;
    private readonly string? userGeneratedWhatIfFile;
    private readonly ICacheHandler? cache;

    public AzureWhatIfHandler(string scopeId,
                              string? resourceGroupName,
                              string template,
                              string parameters,
                              ILogger logger,
                              CommandType commandType,
                              string? location,
                              EstimateOptions options)
    {
        this.scopeId = scopeId;
        this.resourceGroupName = resourceGroupName;
        this.template = template;
        this.deploymentMode = options.Mode;
        this.parameters = parameters;
        this.logger = logger;
        this.commandType = commandType;
        this.location = location;
        this.userGeneratedWhatIfFile = options.UserGeneratedWhatIfFile;

        if(options.DisableCache == false)
        {
            this.cache = options.CacheHandler == CacheHandler.Local ? 
                new LocalCacheHandler(scopeId, resourceGroupName, template, parameters)
                : new AzureStorageCacheHandler(scopeId, resourceGroupName, template, parameters, options.CacheHandlerStorageAccountName!);
        }
    }

    public async Task<WhatIfResponse?> GetResponseWithRetries(CancellationToken token)
    {
        if(token.IsCancellationRequested) return null;

        this.logger.LogInformation("What If status:");
        this.logger.LogInformation("");

        if(this.cache != null)
        {
            var cachedResponse = this.cache.GetCachedData<WhatIfResponse>();
            if(cachedResponse != null)
            {
                this.logger.AddEstimatorMessage("What If data loaded from cache.");
                return cachedResponse;
            }

            this.logger.AddEstimatorMessage("Cache miss for What If data.");
        }
        else
        {
            this.logger.AddEstimatorMessage("Cache is disabled.");
        }

        if(this.userGeneratedWhatIfFile != null)
        {
            this.logger.AddEstimatorMessage("What If data loaded from provided file {0}.", this.userGeneratedWhatIfFile);

            var content = await File.ReadAllTextAsync(this.userGeneratedWhatIfFile, token);
            var properties = JsonSerializer.Deserialize<WhatIfProperties>(content);

            return new WhatIfResponse()
            {
                properties = properties
            };
        }
        
        var response = await SendInitialRequest(token);
        if(response.IsSuccessStatusCode == false)
        {
            var result = await response.Content.ReadAsStringAsync(token);
            this.logger.LogError("{result}", result);

            return null;
        }

        var maxRetries = 5;
        var currentRetry = 1;
        
        while (response.StatusCode == HttpStatusCode.Accepted && currentRetry < maxRetries)
        {
            var retryAfterHeader = response.Headers.RetryAfter;
            var retryAfter = retryAfterHeader == null ? TimeSpan.FromSeconds(15) : retryAfterHeader.Delta;

            this.logger.AddEstimatorMessage("Waiting for response from What If API.");
            await Task.Delay(retryAfter.HasValue ? retryAfter.Value.Seconds * 1000 : 15000, token);

            var location = response.Headers.Location ?? throw new Exception("Location header can't be null when awaiting response.");
            response = await SendAndWaitForResponse(location, token);

            currentRetry++;
        }

#if DEBUG
        var rawData = await response.Content.ReadAsStringAsync();
#endif

        var data = JsonSerializer.Deserialize<WhatIfResponse>(await response.Content.ReadAsStreamAsync());
        if(this.cache != null && data != null)
        {
            this.logger.AddEstimatorMessage("Saving What If response to cache.");
            this.cache.SaveData(data);
        }

        return data;
    }

    private async Task<HttpResponseMessage> SendInitialRequest(CancellationToken cancellationToken)
    {
        var token = GetToken(cancellationToken);
        var request = new HttpRequestMessage(HttpMethod.Post, CreateUrlBasedOnScope());

        string? templateContent;
        if(this.commandType == CommandType.ResourceGroup)
        {
            templateContent = JsonSerializer.Serialize(new EstimatePayload(this.template, this.deploymentMode, this.parameters), new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        }
        else
        {
            templateContent = JsonSerializer.Serialize(new EstimatePayload(this.template, this.deploymentMode, this.parameters, this.location), new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            });
        }

        request.Content = new StringContent(templateContent, Encoding.UTF8, "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.Value.SendAsync(request, cancellationToken);
        return response;
    }

    private string CreateUrlBasedOnScope()
    {
        return this.commandType switch
        {
            CommandType.ResourceGroup => $"https://management.azure.com/subscriptions/{this.scopeId}/resourcegroups/{this.resourceGroupName}/providers/Microsoft.Resources/deployments/arm-estimator/whatIf?api-version=2021-04-01",
            CommandType.Subscription => $"https://management.azure.com/subscriptions/{this.scopeId}/providers/Microsoft.Resources/deployments/arm-estimator/whatIf?api-version=2021-04-01",
            CommandType.ManagementGroup => $"https://management.azure.com/providers/Microsoft.Management/managementGroups/{this.scopeId}/providers/Microsoft.Resources/deployments/arm-estimator/whatIf?api-version=2021-04-01",
            CommandType.Tenant => $"https://management.azure.com/providers/Microsoft.Resources/deployments/arm-estimator/whatIf?api-version=2021-04-01",
            _ => $"https://management.azure.com/subscriptions/{this.scopeId}/resourcegroups/{this.resourceGroupName}/providers/Microsoft.Resources/deployments/arm-estimator/whatIf?api-version=2021-04-01",
        };
    }

    private static async Task<HttpResponseMessage> SendAndWaitForResponse(Uri location, CancellationToken cancellationToken)
    {
        var token = GetToken(cancellationToken);
        var request = new HttpRequestMessage(HttpMethod.Get, location);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.Value.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return response;
    }

    private static string GetToken(CancellationToken cancellationToken)
    {
        var options = new DefaultAzureCredentialOptions()
        {
            ExcludeVisualStudioCodeCredential = true,
            ExcludeVisualStudioCredential = true
        };

        var token = new DefaultAzureCredential(options).GetToken(new TokenRequestContext(new[] { "https://management.azure.com/.default" }), cancellationToken).Token;
        return token;
    }
}
