using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;

namespace ACE.ResourceManager;

internal class CapabilitiesCache
{
    private static readonly Dictionary<string, string> VMSkuOSVhdSize = new Dictionary<string, string>();
    private static readonly Dictionary<string, string> VMSkuPremiumEnabled = new Dictionary<string, string>();

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

            var osVhdSize = sku.Capabilities.Single(_ => _.Name == "OSVhdSizeMB");
            var isPremiumEnabled = sku.Capabilities.Single(_ => _.Name == "PremiumIO");

            VMSkuOSVhdSize.Add(sku.Name, osVhdSize.Value);
            VMSkuPremiumEnabled.Add(sku.Name, isPremiumEnabled.Value);
        }
    }
}
