using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class AKSQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH315HTQSK6";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public AKSQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku;
        sku ??= new WhatIfSku();

        var tier = sku.tier;
        tier ??= "Free";

        var filters = new List<string>();
        var properties = JsonSerializer.Deserialize<AKSProperties>(JsonSerializer.Serialize(this.afterState.properties));
        if (properties != null && properties.AgentPoolProfiles != null)
        {
            foreach (var profile in properties.AgentPoolProfiles)
            {
                if (profile.Type == "VirtualMachineScaleSets")
                {
                    var vmssFilter = new VmssQueryFilter(new WhatIfAfterBeforeChange()
                    {
                        sku = new WhatIfSku()
                        {
                            capacity = profile.Count,
                            name = profile.VmSize,
                            tier = profile.VmSize!.Split('_')[0]
                        },
                        properties = new Dictionary<string, object?>()
                        {
                            {
                                "virtualMachineProfile", JsonSerializer.SerializeToElement(new VirtualMachineProfile()
                                {
                                    hardwareProfile = new VirtualMachineHardwareProfile()
                                    {
                                        vmSize = profile.VmSize
                                    },
                                    storageProfile = new VirtualMachineStorageProfile()
                                    {
                                        imageReference = new VirtualMachineImageReference()
                                        {
                                            offer = profile.OsType
                                        }
                                    }
                                })
                            }
                        }
                    }, this.logger).GetFiltersBasedOnDesiredState(location);

                    if(string.IsNullOrEmpty(vmssFilter))
                    {
                        this.logger.LogWarning("Couldn't generate filter for VMSS for AKS agent profile.");
                        continue;
                    }

                    filters.Add($"({vmssFilter})");
                }
            }
        }

        if (tier == "Free")
        {
            if (filters.Any())
            {
                return string.Join(" or ", filters);
            }
        }

        var skuIds = AKSSupportedData.TierToSkuNameMap[tier];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuName eq '{_}'"));

        if(filters.Any())
        {
            filters.Add($"(serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter}))");
            return string.Join(" or ", filters);
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
