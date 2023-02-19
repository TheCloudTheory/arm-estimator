using ACE.WhatIf;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ACE.Caching
{
    /// <summary>
    /// Implements a cache as a local file based on hash of provided values and WhatIfResponse object.
    /// </summary>
    internal class LocalCacheHandler
    {
        private readonly string key;

        public LocalCacheHandler(string scopeId, string? resourceGroupName, string template, string parameters)
        {
            this.key = GenerateKey(scopeId, resourceGroupName, template, parameters);

            if(Directory.Exists(".ace") == false)
            {
                Directory.CreateDirectory(".ace");
            }
        }

        private string GenerateKey(string scopeId, string? resourceGroupName, string template, string parameters)
        {
            return Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes($"{scopeId}|{resourceGroupName}|{template}|{parameters}")));
        }

        public WhatIfResponse? GetCachedData()
        {
            if(this.CacheFileExists() == false)
            {
                return null;
            }

            var content = File.ReadAllText(Path.Combine(".ace", $"{this.key}.cache"));
            if(string.IsNullOrEmpty(content))
            {
                return null;
            }

            return JsonSerializer.Deserialize<WhatIfResponse>(content);
        }

        public void SaveData(WhatIfResponse data)
        {
            File.WriteAllText(Path.Combine(".ace", $"{this.key}.cache"), JsonSerializer.Serialize(data));
        }

        private bool CacheFileExists()
        {
            return File.Exists(Path.Combine(".ace", $"{this.key}.cache"));
        }
    }
}
