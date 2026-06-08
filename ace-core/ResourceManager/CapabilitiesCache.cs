using ACE.Caching;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;

namespace ACE.ResourceManager;

internal class CapabilitiesCache
{
    private readonly ICacheHandler cache;

    public CapabilitiesCache(CacheHandler cacheHandler, string? cacheHandlerStorageAccountName)
    {
        this.cache = cacheHandler == CacheHandler.Local ? 
            new LocalCacheHandler() :
            new AzureStorageCacheHandler("vm", cacheHandlerStorageAccountName!);
    }

    /// <summary>
    /// This method initializes capabilities cache for VMs. That cache is used when calculating 
    /// disk cost for virtual machines - if a template doesn't contain information about OS disk
    /// configuration, Azure infers it based on the internal configuration. This includes two
    /// parameters:
    /// - which disk type is used (Standard HDD / Standard SSD / Premium SSD)
    /// - what disk size should be used
    /// 
    /// The former is based on 'PremiumIO' capability, the latter on 'OSVhdSizeMB'. It's important
    /// to remember, that Azure chooses disks with the best performance, i.e. if VM supports
    /// Premium SSD, inferred disk type will be exactly that.
    /// </summary>
    public void InitializeVirtualMachineCapabilities(string location, CancellationToken token)
    {
        if(token.IsCancellationRequested) return;
        if(this.cache.CacheFileExists())
        {
            return;
        }

        var credentials = new DefaultAzureCredential();
        var client = new ArmClient(credentials);
        var defaultSubscription = client.GetDefaultSubscription(token);
        var vmSkuPremiumEnabled = new Dictionary<string, string>();
        
        foreach (var sku in defaultSubscription.GetComputeResourceSkus(filter: $"location eq '{location}'", cancellationToken: token))
        {
            if (sku.ResourceType != "virtualMachines") continue;

            var isPremiumEnabled = sku.Capabilities.Single(_ => _.Name == "PremiumIO");

            vmSkuPremiumEnabled.Add(sku.Name, isPremiumEnabled.Value);
        }

        this.cache.SaveData(vmSkuPremiumEnabled);
    }

    public Dictionary<string, string>? GetCapabilities()
    {
        return this.cache.GetCachedData<Dictionary<string, string>>();
    }
}
