using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class VmssQueryFilter : IQueryFilter
{
    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public VmssQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for VMSS when SKU is unavailable.");
            return null;
        }

        if (this.afterState.properties == null || this.afterState.properties.ContainsKey("virtualMachineProfile") == false)
        {
            this.logger.LogError("Can't create a filter for VMSS when VM profile is unavailable.");
            return null;
        }

        if(this.afterState.properties["virtualMachineProfile"] == null)
        {
            this.logger.LogError("Can't create a filter for VMSS when VM profile is unavailable.");
            return null;
        }

        var vmProfile = ((JsonElement)this.afterState.properties["virtualMachineProfile"]!).Deserialize<VirtualMachineProfile>();
        if (vmProfile == null)
        {
            this.logger.LogError("Can't create a filter for VMSS when VM profile is unavailable.");
            return null;
        }

        var vmFilter = new VirtualMachineQueryFilter(new WhatIfAfterBeforeChange()
        {
            sku = sku,
            properties = new Dictionary<string, object?>()
            {
                { "hardwareProfile", JsonSerializer.SerializeToElement(new VirtualMachineHardwareProfile() {
                    vmSize = sku.name
                }) },
                { "storageProfile", JsonSerializer.SerializeToElement(vmProfile.storageProfile) },
                { "priority", vmProfile.priority }
            }
        }, this.logger).GetFiltersBasedOnDesiredState(location);

        return vmFilter;
    }
}
