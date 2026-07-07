using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ACE.Caching;

/// <summary>
/// Implements a cache as a local file based on hash of provided values and WhatIfResponse object.
/// </summary>
internal class LocalCacheHandler : ICacheHandler
{
    private static readonly string CacheDirectory =
        Path.Combine(Path.GetTempPath(), ".ace");

    private readonly string key;

    static LocalCacheHandler()
    {
        if (Directory.Exists(CacheDirectory) == false)
        {
            Directory.CreateDirectory(CacheDirectory);
        }
    }

    public LocalCacheHandler(string scopeId, string? resourceGroupName, string template, ParametersSchema parameters)
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

    private string GenerateKey(string scopeId, string? resourceGroupName, string template, ParametersSchema parameters)
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

        var content = File.ReadAllText(Path.Combine(CacheDirectory, $"{this.key}.cache"));
        if(string.IsNullOrEmpty(content))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(content);
    }

    public void SaveData(object data)
    {
        File.WriteAllText(Path.Combine(CacheDirectory, $"{this.key}.cache"), JsonSerializer.Serialize(data));
    }

    public bool CacheFileExists()
    {
        return File.Exists(Path.Combine(CacheDirectory, $"{this.key}.cache"));
    }
}