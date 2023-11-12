using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ACE.Caching;

/// <summary>
/// Implements a cache as a local file based on hash of provided values and WhatIfResponse object.
/// </summary>
internal class LocalCacheHandler : ICacheHandler
{
    private readonly string key;

    static LocalCacheHandler()
    {
        if (Directory.Exists(".ace") == false)
        {
            Directory.CreateDirectory(".ace");
        }
    }

    public LocalCacheHandler(string scopeId, string? resourceGroupName, string template, string parameters)
    {
        this.key = this.GenerateKey(scopeId, resourceGroupName, template, parameters);
    }

    /// <summary>
    /// Initializes default version of cache handler
    /// </summary>
    public LocalCacheHandler()
    {
        this.key = this.GenerateKey("vm");
    }

    private string GenerateKey(string scopeId, string? resourceGroupName, string template, string parameters)
    {
        return this.GenerateKey($"{scopeId}|{resourceGroupName}|{template}|{parameters}");
    }
    
    private string GenerateKey(string value)
    {
        return Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(value)));
    }

    public T? GetCachedData<T>() where T : class
    {
        if(this.CacheFileExists() == false)
        {
            return default;
        }

        var content = File.ReadAllText(Path.Combine(".ace", $"{this.key}.cache"));
        if(string.IsNullOrEmpty(content))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(content);
    }

    public void SaveData(object data)
    {
        File.WriteAllText(Path.Combine(".ace", $"{this.key}.cache"), JsonSerializer.Serialize(data));
    }

    public bool CacheFileExists()
    {
        return File.Exists(Path.Combine(".ace", $"{this.key}.cache"));
    }
}