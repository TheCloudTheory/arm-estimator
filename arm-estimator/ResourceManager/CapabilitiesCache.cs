using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using System.Collections.Concurrent;

namespace ACE.ResourceManager;

internal class CapabilitiesCache
{
    private static readonly ConcurrentDictionary<string, string> _VMSkuPremiumEnabled = new();

    public static IReadOnlyDictionary<string, string> VMSkuPremiumEnabled => _VMSkuPremiumEnabled;

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
    public static void InitializeVirtualMachineCapabilities(string location)
    {
        var credentials = new DefaultAzureCredential();
        var client = new ArmClient(credentials);
        var defaultSubscription = client.GetDefaultSubscription();
        
        foreach (var sku in defaultSubscription.GetComputeResourceSkus(filter: $"location eq '{location}'"))
        {
            if (sku.ResourceType != "virtualMachines") continue;

            var isPremiumEnabled = sku.Capabilities.Single(_ => _.Name == "PremiumIO");

            _VMSkuPremiumEnabled.TryAdd(sku.Name, isPremiumEnabled.Value);
        }
    }
}
