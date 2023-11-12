using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ACE.Caching;

internal class AzureStorageCacheHandler : ICacheHandler
{
    private readonly string key;
    private readonly BlobContainerClient client;

    private AzureStorageCacheHandler(string accountName)
    {
        ArgumentNullException.ThrowIfNull(accountName, nameof(accountName));
        
        var accountUri = new Uri($"https://{accountName}.blob.core.windows.net");

        var serviceClient = new BlobServiceClient(accountUri, new DefaultAzureCredential());
        this.client = serviceClient.GetBlobContainerClient("acecache");
    
        if(this.client.Exists() == false)
        {
            this.client.Create(PublicAccessType.None);
        }

        this.key = string.Empty;
    }

    public AzureStorageCacheHandler(string key, string accountName)
        : this(accountName)
    {
        this.key = this.GenerateKey(key);
    }

    public AzureStorageCacheHandler(string scopeId, string? resourceGroupName, string template, string parameters, string accountName)
        : this(accountName)
    {
        this.key = this.GenerateKey(scopeId, resourceGroupName, template, parameters);
    }

    private string GenerateKey(string scopeId, string? resourceGroupName, string template, string parameters)
    {
        return this.GenerateKey($"{scopeId}|{resourceGroupName}|{template}|{parameters}");
    }
    
    private string GenerateKey(string value)
    {
        return Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(value)));
    }

    public bool CacheFileExists()
    {
        return this.client.GetBlobClient($"{this.key}.cache").Exists();
    }

    public T? GetCachedData<T>() where T : class
    {
        if(this.CacheFileExists() == false)
        {
            return default;
        }

        var content = this.client.GetBlobClient($"{this.key}.cache").DownloadContent().Value.Content.ToString();
        if(string.IsNullOrEmpty(content))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(content);
    }

    public void SaveData(object data)
    {
        this.client.GetBlobClient($"{this.key}.cache").Upload(new BinaryData(JsonSerializer.Serialize(data)));
    }
}
