using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class VirtualMachineQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH313Z7MMC8";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public VirtualMachineQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        string? vmSize = null;
        bool? isWindows = true;

        if(this.afterState.properties != null && this.afterState.properties.ContainsKey("hardwareProfile"))
        {
            var hardwareProfile = ((JsonElement)this.afterState.properties["hardwareProfile"]).Deserialize<VirtualMachineHardwareProfile>();
            vmSize = hardwareProfile?.vmSize;
        }

        if (this.afterState.properties != null && this.afterState.properties.ContainsKey("storageProfile"))
        {
            var storageProfile = ((JsonElement)this.afterState.properties["storageProfile"]).Deserialize<VirtualMachineStorageProfile>();
            isWindows = storageProfile?.imageReference?.offer?.Contains("Windows");
        }

        if (vmSize == null || isWindows == null)
        {
            this.logger.LogError("Can't create a filter for Virtual Machine when VM size or OS is unavailable.");
            return null;
        }

        var sku = isWindows.Value ? $"{vmSize}_Windows" : $"{vmSize}_Linux";
        var skuIds = VirtualMachineSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
