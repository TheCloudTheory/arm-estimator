using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Cryptography;
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

        if(isWindows == null)
        {
            isWindows = true;
        }

        if (vmSize == null || isWindows == null)
        {
            this.logger.LogError("Can't create a filter for Virtual Machine when VM size or OS is unavailable.");
            return null;
        }

        var vmSizeParts = vmSize.Split("_");
        var familySku = vmSizeParts.Length == 3 ? vmSizeParts[1] + vmSizeParts[2] : vmSizeParts[1];
        var tier = vmSizeParts[0];
        var os = isWindows.Value ? "Windows" : "Linux";
        string? skuName;
        string? productName;

        if(tier == "Basic" && os == "Windows")
        {
            productName = "Virtual Machines A Series Basic Windows";
            skuName = vmSizeParts[1];
        }
        else if (tier == "Basic" && os == "Linux")
        {
            productName = "Virtual Machines A Series Basic";
            skuName = vmSizeParts[1];
        }
        else
        {
            switch (familySku)
            {
                case "A0":
                case "A1":
                case "A2":
                case "A3":
                case "A4":
                case "A5":
                case "A6":
                case "A7":
                    familySku = "A";
                    skuName = vmSizeParts[1];
                    break;
                case "A1v2":
                case "A2v2":
                case "A3v2":
                case "A4v2":
                case "A5v2":
                case "A6v2":
                case "A7v2":
                case "A8v2":
                    familySku = "Av2";
                    skuName = vmSizeParts[1] + " " + vmSizeParts[2];
                    break;
                case "A2mv2":
                case "A4mv2":
                case "A8mv2":
                    familySku = "Av2";
                    skuName = vmSizeParts[1] + " " + vmSizeParts[2];
                    break;
                case "B1s":
                case "B1ls":
                case "B1ms":
                case "B2s":
                case "B2ms":
                case "B4ms":
                case "B8ms":
                case "B12ms":
                case "B16ms":
                case "B20ms":
                    familySku = "BS";
                    skuName = vmSizeParts[1];
                    break;
                case "D1":
                    familySku = "D";
                    skuName = $"D1/DS1";
                    break;
                case "D2":
                    familySku = "D";
                    skuName = $"D2/DS2";
                    break;
                case "D3":
                    familySku = "D";
                    skuName = $"D3/DS3";
                    break;
                case "D4":
                    familySku = "D";
                    skuName = $"D4/DS4";
                    break;
                case "D11":
                    familySku = "D";
                    skuName = $"D11/DS11";
                    break;
                case "D12":
                    familySku = "D";
                    skuName = $"D12/DS12";
                    break;
                case "D13":
                    familySku = "D";
                    skuName = $"D13/DS13";
                    break;
                case "D14":
                    familySku = "D";
                    skuName = $"D14/DS14";
                    break;
                case "D1 v2":
                    familySku = "Dv2";
                    skuName = $"D1 v2/DS1 v2";
                    break;
                case "D2 v2":
                    familySku = "Dv2";
                    skuName = $"D2 v2/DS2 v2";
                    break;
                case "D3 v2":
                    familySku = "Dv2";
                    skuName = $"D3 v2/DS3 v2";
                    break;
                case "D4 v2":
                    familySku = "Dv2";
                    skuName = $"D4 v2/DS4 v2";
                    break;
                case "D5 v2":
                    familySku = "Dv2";
                    skuName = $"D5 v2/DS5 v2";
                    break;
                case "D11 v2":
                    familySku = "Dv2";
                    skuName = $"D11 v2/DS11 v2";
                    break;
                case "D12 v2":
                    familySku = "Dv2";
                    skuName = $"D12 v2/DS12 v2";
                    break;
                case "D13 v2":
                    familySku = "Dv2";
                    skuName = $"D13 v2/DS13 v2";
                    break;
                case "D14 v2":
                    familySku = "Dv2";
                    skuName = $"D14 v2/DS14 v2";
                    break;
                case "D15 v2":
                    familySku = "Dv2";
                    skuName = $"D15 v2/DS15 v2";
                    break;
                case "D15i v2":
                    familySku = "Dv2";
                    skuName = $"D15i v2/DS15i v2";
                    break;
                case "D2 v3":
                    familySku = "Dv3";
                    skuName = $"D2 v3/D2s v3";
                    break;
                case "D4 v3":
                    familySku = "Dv3";
                    skuName = $"D4 v3/D4s v3";
                    break;
                case "D8 v3":
                    familySku = "Dv3";
                    skuName = $"D8 v3/D8s v3";
                    break;
                case "D16 v3":
                    familySku = "Dv3";
                    skuName = $"D16 v3/D16s v3";
                    break;
                case "D32 v3":
                    familySku = "Dv3";
                    skuName = $"D32 v3/D32s v3";
                    break;
                case "D48 v3":
                    familySku = "Dv3";
                    skuName = $"D48 v3/D48s v3";
                    break;
                case "D64 v3":
                    familySku = "Dv3";
                    skuName = $"D64 v3/D64s v3";
                    break;
                case "D2 v4":
                    familySku = "Dv4";
                    skuName = $"D2 v4";
                    break;
                case "D4 v4":
                    familySku = "Dv4";
                    skuName = $"D4 v4";
                    break;
                case "D8 v4":
                    familySku = "Dv4";
                    skuName = $"D8 v4";
                    break;
                case "D32 v4":
                    familySku = "Dv4";
                    skuName = $"D32 v4";
                    break;
                case "D48 v4":
                    familySku = "Dv4";
                    skuName = $"D48 v4";
                    break;
                case "D64 v4":
                    familySku = "Dv4";
                    skuName = $"D64 v4";
                    break;
                case "D2a v4":
                    familySku = "Dav4";
                    skuName = $"D2a v4/D2as v4";
                    break;
                case "D4a v4":
                    familySku = "Dav4";
                    skuName = $"D4a v4/D4as v4";
                    break;
                case "D8a v4":
                    familySku = "Dav4";
                    skuName = $"D8a v4/D8as v4";
                    break;
                case "D16a v4":
                    familySku = "Dav4";
                    skuName = $"D16a v4/D16as v4";
                    break;
                case "D32a v4":
                    familySku = "Dav4";
                    skuName = $"D32a v4/D32as v4";
                    break;
                case "D48a v4":
                    familySku = "Dav4";
                    skuName = $"D48a v4/D48as v4";
                    break;
                case "D64a v4":
                    familySku = "Dav4";
                    skuName = $"D64a v4/D64as v4";
                    break;
                case "D96a v4":
                    familySku = "Dav4";
                    skuName = $"D96a v4/D96as v4";
                    break;
                case "D2as v4":
                    familySku = "Dasv4";
                    skuName = $"D2as v4";
                    break;
                case "D4as v4":
                    familySku = "Dasv4";
                    skuName = $"D4as v4";
                    break;
                case "D8as v4":
                    familySku = "Dasv4";
                    skuName = $"D8as v4";
                    break;
                case "D16as v4":
                    familySku = "Dasv4";
                    skuName = $"D16as v4";
                    break;
                case "D32as v4":
                    familySku = "Dasv4";
                    skuName = $"D32as v4";
                    break;
                case "D48as v4":
                    familySku = "Dasv4";
                    skuName = $"D48as v4";
                    break;
                case "D64as v4":
                    familySku = "Dasv4";
                    skuName = $"D64as v4";
                    break;
                case "D96as v4":
                    familySku = "Dasv4";
                    skuName = $"D96as v4";
                    break;
                case "D2d v4":
                    familySku = "Ddv4";
                    skuName = $"D2d v4";
                    break;
                case "D4d v4":
                    familySku = "Ddv4";
                    skuName = $"D4d v4";
                    break;
                case "D8d v4":
                    familySku = "Ddv4";
                    skuName = $"D8d v4";
                    break;
                case "D16d v4":
                    familySku = "Ddv4";
                    skuName = $"D16d v4";
                    break;
                case "D32d v4":
                    familySku = "Ddv4";
                    skuName = $"D32d v4";
                    break;
                case "D64d v4":
                    familySku = "Ddv4";
                    skuName = $"D64d v4";
                    break;
                case "D2ds v4":
                    familySku = "Ddsv4";
                    skuName = $"D2ds v4";
                    break;
                case "D4ds v4":
                    familySku = "Ddsv4";
                    skuName = $"D4ds v4";
                    break;
                case "D8ds v4":
                    familySku = "Ddsv4";
                    skuName = $"D8ds v4";
                    break;
                case "D16ds v4":
                    familySku = "Ddsv4";
                    skuName = $"D16ds v4";
                    break;
                case "D32ds v4":
                    familySku = "Ddsv4";
                    skuName = $"D32ds v4";
                    break;
                case "D48ds v4":
                    familySku = "Ddsv4";
                    skuName = $"D48ds v4";
                    break;
                case "D64ds v4":
                    familySku = "Ddsv4";
                    skuName = $"D64ds v4";
                    break;
                case "D2s v3":
                    familySku = "DSv3";
                    skuName = $"D2s v3";
                    break;
                case "D4s v3":
                    familySku = "DSv3";
                    skuName = $"D4s v3";
                    break;
                case "D8s v3":
                    familySku = "DSv3";
                    skuName = $"D8s v3";
                    break;
                case "D16s v3":
                    familySku = "DSv3";
                    skuName = $"D16s v3";
                    break;
                case "D32s v3":
                    familySku = "DSv3";
                    skuName = $"D32s v3";
                    break;
                case "D32-8s v3":
                    familySku = "DSv3";
                    skuName = $"D32-8s v3";
                    break;
                case "D32-16s v3":
                    familySku = "DSv3";
                    skuName = $"D32-16s v3";
                    break;
                case "D48s v3":
                    familySku = "DSv3";
                    skuName = $"D48s v3";
                    break;
                case "D64s v3":
                    familySku = "DSv3";
                    skuName = $"D64s v3";
                    break;
                case "D64-16s v3":
                    familySku = "DSv3";
                    skuName = $"D64-16s v3";
                    break;
                case "D64-32s v3":
                    familySku = "DSv3";
                    skuName = $"D64-32s v3";
                    break;
                case "D2s v4":
                    familySku = "Dsv4";
                    skuName = $"D2s v4";
                    break;
                case "D4s v4":
                    familySku = "Dsv4";
                    skuName = $"D4s v4";
                    break;
                case "D8s v4":
                    familySku = "Dsv4";
                    skuName = $"D8s v4";
                    break;
                case "D16s v4":
                    familySku = "Dsv4";
                    skuName = $"D16s v4";
                    break;
                case "D32s v4":
                    familySku = "Dsv4";
                    skuName = $"D32s v4";
                    break;
                case "D48s v4":
                    familySku = "Dsv4";
                    skuName = $"D48s v4";
                    break;
                case "D64s v4":
                    familySku = "Dsv4";
                    skuName = $"D64s v4";
                    break;
                default:
                    this.logger.LogWarning("VM size {size} is not supported.", vmSize);
                    return null;
            }

            if(os == "Windows")
            {
                productName = $"Virtual Machines {familySku} Series Windows";
            }
            else
            {
                productName = $"Virtual Machines {familySku} Series";
            }
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}'";
    }
}
