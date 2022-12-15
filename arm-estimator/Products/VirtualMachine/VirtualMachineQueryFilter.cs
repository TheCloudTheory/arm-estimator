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
                case "D1v2":
                    familySku = "Dv2";
                    skuName = $"D1 v2";
                    break;
                case "D2v2":
                    familySku = "Dv2";
                    skuName = $"D2 v2";
                    break;
                case "D3v2":
                    familySku = "Dv2";
                    skuName = $"D3 v2";
                    break;
                case "D4v2":
                    familySku = "Dv2";
                    skuName = $"D4 v2";
                    break;
                case "D5v2":
                    familySku = "Dv2";
                    skuName = $"D5 v2";
                    break;
                case "D11v2":
                    familySku = "Dv2";
                    skuName = $"D11 v2";
                    break;
                case "D12v2":
                    familySku = "Dv2";
                    skuName = $"D12 v2";
                    break;
                case "D13v2":
                    familySku = "Dv2";
                    skuName = $"D13 v2";
                    break;
                case "D14v2":
                    familySku = "Dv2";
                    skuName = $"D14 v2";
                    break;
                case "D15v2":
                    familySku = "Dv2";
                    skuName = $"D15 v2";
                    break;
                case "D15iv2":
                    familySku = "Dv2";
                    skuName = $"D15i v2";
                    break;
                case "D2v3":
                    familySku = "Dv3";
                    skuName = $"D2 v3";
                    break;
                case "D4v3":
                    familySku = "Dv3";
                    skuName = $"D4 v3";
                    break;
                case "D8v3":
                    familySku = "Dv3";
                    skuName = $"D8 v3";
                    break;
                case "D16v3":
                    familySku = "Dv3";
                    skuName = $"D16 v3";
                    break;
                case "D32v3":
                    familySku = "Dv3";
                    skuName = $"D32 v3";
                    break;
                case "D48v3":
                    familySku = "Dv3";
                    skuName = $"D48 v3";
                    break;
                case "D64v3":
                    familySku = "Dv3";
                    skuName = $"D64 v3";
                    break;
                case "D2v4":
                    familySku = "Dv4";
                    skuName = $"D2 v4";
                    break;
                case "D4v4":
                    familySku = "Dv4";
                    skuName = $"D4 v4";
                    break;
                case "D8v4":
                    familySku = "Dv4";
                    skuName = $"D8 v4";
                    break;
                case "D32v4":
                    familySku = "Dv4";
                    skuName = $"D32 v4";
                    break;
                case "D48v4":
                    familySku = "Dv4";
                    skuName = $"D48 v4";
                    break;
                case "D64v4":
                    familySku = "Dv4";
                    skuName = $"D64 v4";
                    break;
                case "D2av4":
                    familySku = "Dav4";
                    skuName = $"D2a v4";
                    break;
                case "D4av4":
                    familySku = "Dav4";
                    skuName = $"D4a v4";
                    break;
                case "D8av4":
                    familySku = "Dav4";
                    skuName = $"D8a v4";
                    break;
                case "D16av4":
                    familySku = "Dav4";
                    skuName = $"D16a v4";
                    break;
                case "D32av4":
                    familySku = "Dav4";
                    skuName = $"D32a v4";
                    break;
                case "D48av4":
                    familySku = "Dav4";
                    skuName = $"D48a v4";
                    break;
                case "D64av4":
                    familySku = "Dav4";
                    skuName = $"D64a v4";
                    break;
                case "D96av4":
                    familySku = "Dav4";
                    skuName = $"D96a v4";
                    break;
                case "D2asv4":
                    familySku = "Dasv4";
                    skuName = $"D2as v4";
                    break;
                case "D4asv4":
                    familySku = "Dasv4";
                    skuName = $"D4as v4";
                    break;
                case "D8asv4":
                    familySku = "Dasv4";
                    skuName = $"D8as v4";
                    break;
                case "D16asv4":
                    familySku = "Dasv4";
                    skuName = $"D16as v4";
                    break;
                case "D32asv4":
                    familySku = "Dasv4";
                    skuName = $"D32as v4";
                    break;
                case "D48asv4":
                    familySku = "Dasv4";
                    skuName = $"D48as v4";
                    break;
                case "D64asv4":
                    familySku = "Dasv4";
                    skuName = $"D64as v4";
                    break;
                case "D96asv4":
                    familySku = "Dasv4";
                    skuName = $"D96as v4";
                    break;
                case "D2dv4":
                    familySku = "Ddv4";
                    skuName = $"D2d v4";
                    break;
                case "D4dv4":
                    familySku = "Ddv4";
                    skuName = $"D4d v4";
                    break;
                case "D8dv4":
                    familySku = "Ddv4";
                    skuName = $"D8d v4";
                    break;
                case "D16dv4":
                    familySku = "Ddv4";
                    skuName = $"D16d v4";
                    break;
                case "D32dv4":
                    familySku = "Ddv4";
                    skuName = $"D32d v4";
                    break;
                case "D64dv4":
                    familySku = "Ddv4";
                    skuName = $"D64d v4";
                    break;
                case "D2dsv4":
                    familySku = "Ddsv4";
                    skuName = $"D2ds v4";
                    break;
                case "D4dsv4":
                    familySku = "Ddsv4";
                    skuName = $"D4ds v4";
                    break;
                case "D8dsv4":
                    familySku = "Ddsv4";
                    skuName = $"D8ds v4";
                    break;
                case "D16dsv4":
                    familySku = "Ddsv4";
                    skuName = $"D16ds v4";
                    break;
                case "D32dsv4":
                    familySku = "Ddsv4";
                    skuName = $"D32ds v4";
                    break;
                case "D48dsv4":
                    familySku = "Ddsv4";
                    skuName = $"D48ds v4";
                    break;
                case "D64dsv4":
                    familySku = "Ddsv4";
                    skuName = $"D64ds v4";
                    break;
                case "D2sv3":
                    familySku = "DSv3";
                    skuName = $"D2s v3";
                    break;
                case "D4sv3":
                    familySku = "DSv3";
                    skuName = $"D4s v3";
                    break;
                case "D8sv3":
                    familySku = "DSv3";
                    skuName = $"D8s v3";
                    break;
                case "D16sv3":
                    familySku = "DSv3";
                    skuName = $"D16s v3";
                    break;
                case "D32sv3":
                    familySku = "DSv3";
                    skuName = $"D32s v3";
                    break;
                case "D32-8sv3":
                    familySku = "DSv3";
                    skuName = $"D32-8s v3";
                    break;
                case "D32-16sv3":
                    familySku = "DSv3";
                    skuName = $"D32-16s v3";
                    break;
                case "D48sv3":
                    familySku = "DSv3";
                    skuName = $"D48s v3";
                    break;
                case "D64sv3":
                    familySku = "DSv3";
                    skuName = $"D64s v3";
                    break;
                case "D64-16sv3":
                    familySku = "DSv3";
                    skuName = $"D64-16s v3";
                    break;
                case "D64-32sv3":
                    familySku = "DSv3";
                    skuName = $"D64-32s v3";
                    break;
                case "D2sv4":
                    familySku = "Dsv4";
                    skuName = $"D2s v4";
                    break;
                case "D4sv4":
                    familySku = "Dsv4";
                    skuName = $"D4s v4";
                    break;
                case "D8sv4":
                    familySku = "Dsv4";
                    skuName = $"D8s v4";
                    break;
                case "D16sv4":
                    familySku = "Dsv4";
                    skuName = $"D16s v4";
                    break;
                case "D32sv4":
                    familySku = "Dsv4";
                    skuName = $"D32s v4";
                    break;
                case "D48sv4":
                    familySku = "Dsv4";
                    skuName = $"D48s v4";
                    break;
                case "D64sv4":
                    familySku = "Dsv4";
                    skuName = $"D64s v4";
                    break;
                case "D1s":
                    familySku = "DS";
                    skuName = $"DS1";
                    break;
                case "D2s":
                    familySku = "DS";
                    skuName = $"DS2";
                    break;
                case "D3s":
                    familySku = "DS";
                    skuName = $"DS3";
                    break;
                case "D4s":
                    familySku = "DS";
                    skuName = $"DS4";
                    break;
                case "D11s":
                    familySku = "DS";
                    skuName = $"DS11";
                    break;
                case "D12s":
                    familySku = "DS";
                    skuName = $"DS12";
                    break;
                case "D13s":
                    familySku = "DS";
                    skuName = $"DS13";
                    break;
                case "D14s":
                    familySku = "DS";
                    skuName = $"DS14";
                    break;
                case "D2asv5":
                    familySku = "Dasv5";
                    skuName = $"Standard_D2as_v5";
                    break;
                case "D4asv5":
                    familySku = "Dasv5";
                    skuName = $"Standard_D4as_v5";
                    break;
                case "D8asv5":
                    familySku = "Dasv5";
                    skuName = $"Standard_D8as_v5";
                    break;
                case "D16asv5":
                    familySku = "Dasv5";
                    skuName = $"Standard_D16as_v5";
                    break;
                case "D32asv5":
                    familySku = "Dasv5";
                    skuName = $"Standard_D32as_v5";
                    break;
                case "D48asv5":
                    familySku = "Dasv5";
                    skuName = $"Standard_D48as_v5";
                    break;
                case "D96asv5":
                    familySku = "Dasv5";
                    skuName = $"Standard_D96as_v5";
                    break;
                case "D2v5":
                    familySku = "Dv5";
                    skuName = $"Standard_D2_v5";
                    break;
                case "D4v5":
                    familySku = "Dv5";
                    skuName = $"Standard_D4_v5";
                    break;
                case "D8v5":
                    familySku = "Dv5";
                    skuName = $"Standard_D8_v5";
                    break;
                case "D16v5":
                    familySku = "Dv5";
                    skuName = $"Standard_D16_v5";
                    break;
                case "D32v5":
                    familySku = "Dv5";
                    skuName = $"Standard_D32_v5";
                    break;
                case "D48v5":
                    familySku = "Dv5";
                    skuName = $"Standard_D48_v5";
                    break;
                case "D96v5":
                    familySku = "Dv5";
                    skuName = $"Standard_D96_v5";
                    break;
                case "D2adsv5":
                    familySku = "Dadsv5";
                    skuName = $"Standard_D2ads_v5";
                    break;
                case "D4adsv5":
                    familySku = "Dadsv5";
                    skuName = $"Standard_D4ads_v5";
                    break;
                case "D8adsv5":
                    familySku = "Dadsv5";
                    skuName = $"Standard_D8ads_v5";
                    break;
                case "D16adsv5":
                    familySku = "Dadsv5";
                    skuName = $"Standard_D16ads_v5";
                    break;
                case "D32adsv5":
                    familySku = "Dadsv5";
                    skuName = $"Standard_D32ads_v5";
                    break;
                case "D48adsv5":
                    familySku = "Dadsv5";
                    skuName = $"Standard_D48ads_v5";
                    break;
                case "D96adsv5":
                    familySku = "Dadsv5";
                    skuName = $"Standard_D96ads_v5";
                    break;
                case "D2dv5":
                    familySku = "Ddv5";
                    skuName = $"Standard_D2d_v5";
                    break;
                case "D4dv5":
                    familySku = "Ddv5";
                    skuName = $"Standard_D4d_v5";
                    break;
                case "D8dv5":
                    familySku = "Ddv5";
                    skuName = $"Standard_D8d_v5";
                    break;
                case "D16dv5":
                    familySku = "Ddv5";
                    skuName = $"Standard_D16d_v5";
                    break;
                case "D32dv5":
                    familySku = "Ddv5";
                    skuName = $"Standard_D32d_v5";
                    break;
                case "D48dv5":
                    familySku = "Ddv5";
                    skuName = $"Standard_D48d_v5";
                    break;
                case "D96dv5":
                    familySku = "Ddsv5";
                    skuName = $"Standard_D96d_v5";
                    break;
                case "D2dsv5":
                    familySku = "Ddsv5";
                    skuName = $"Standard_D2ds_v5";
                    break;
                case "D4dsv5":
                    familySku = "Ddsv5";
                    skuName = $"Standard_D4ds_v5";
                    break;
                case "D8dsv5":
                    familySku = "Ddsv5";
                    skuName = $"Standard_D8ds_v5";
                    break;
                case "D16dsv5":
                    familySku = "Ddsv5";
                    skuName = $"Standard_D16ds_v5";
                    break;
                case "D32dsv5":
                    familySku = "Ddsv5";
                    skuName = $"Standard_D32ds_v5";
                    break;
                case "D48dsv5":
                    familySku = "Ddsv5";
                    skuName = $"Standard_D48ds_v5";
                    break;
                case "D96dsv5":
                    familySku = "Ddsv5";
                    skuName = $"Standard_D96ds_v5";
                    break;
                case "DC1sv2":
                    familySku = "DCSv2";
                    skuName = $"DC1s v2";
                    break;
                case "DC2sv2":
                    familySku = "DCSv2";
                    skuName = $"DC2s v2";
                    break;
                case "DC4sv2":
                    familySku = "DCSv2";
                    skuName = $"DC4s v2";
                    break;
                case "DC8sv2":
                    familySku = "DCSv2";
                    skuName = $"DC8s v2";
                    break;
                case "DC1sv3":
                    familySku = "DCsv3";
                    skuName = $"standard_DC1s_v3";
                    break;
                case "DC2sv3":
                    familySku = "DCsv3";
                    skuName = $"standard_DC2s_v3";
                    break;
                case "DC4sv3":
                    familySku = "DCsv3";
                    skuName = $"standard_DC4s_v3";
                    break;
                case "DC8sv3":
                    familySku = "DCsv3";
                    skuName = $"standard_DC8s_v3";
                    break;
                case "DC16sv3":
                    familySku = "DCsv3";
                    skuName = $"standard_DC16s_v3";
                    break;
                case "DC32sv3":
                    familySku = "DCsv3";
                    skuName = $"standard_DC32s_v3";
                    break;
                case "DC48sv3":
                    familySku = "DCsv3";
                    skuName = $"standard_DC48s_v3";
                    break;
                case "DC1dsv3":
                    familySku = "DCdsv3";
                    skuName = $"standard_DC1ds_v3";
                    break;
                case "DC2dsv3":
                    familySku = "DCdsv3";
                    skuName = $"standard_DC2ds_v3";
                    break;
                case "DC4dsv3":
                    familySku = "DCdsv3";
                    skuName = $"standard_DC4ds_v3";
                    break;
                case "DC8dsv3":
                    familySku = "DCdsv3";
                    skuName = $"standard_DC8ds_v3";
                    break;
                case "DC16dsv3":
                    familySku = "DCdsv3";
                    skuName = $"standard_DC16ds_v3";
                    break;
                case "DC32dsv3":
                    familySku = "DCdsv3";
                    skuName = $"standard_DC32ds_v3";
                    break;
                case "DC48dsv3":
                    familySku = "DCdsv3";
                    skuName = $"standard_DC48ds_v3";
                    break;
                case "DC2adsv5":
                    familySku = "DCadsv5-";
                    skuName = $"Standard_DC2ads_v5";
                    break;
                case "DC4adsv5":
                    familySku = "DCadsv5-";
                    skuName = $"Standard_DC4ads_v5";
                    break;
                case "DC8adsv5":
                    familySku = "DCadsv5-";
                    skuName = $"Standard_DC8ads_v5";
                    break;
                case "DC16adsv5":
                    familySku = "DCadsv5-";
                    skuName = $"Standard_DC16ads_v5";
                    break;
                case "DC32adsv5":
                    familySku = "DCadsv5-";
                    skuName = $"Standard_DC32ads_v5";
                    break;
                case "DC48adsv5":
                    familySku = "DCadsv5-";
                    skuName = $"Standard_DC48ads_v5";
                    break;
                case "DC96adsv5":
                    familySku = "DCadsv5-";
                    skuName = $"Standard_DC96ads_v5";
                    break;
                case "DS1v2":
                    familySku = "DSv2";
                    skuName = $"DS1 v2";
                    break;
                case "DS2v2":
                    familySku = "DSv2";
                    skuName = $"DS2 v2";
                    break;
                case "DS3v2":
                    familySku = "DSv2";
                    skuName = $"DS3 v2";
                    break;
                case "DS4v2":
                    familySku = "DSv2";
                    skuName = $"DS4 v2";
                    break;
                case "DS5v2":
                    familySku = "DSv2";
                    skuName = $"DS5 v2";
                    break;
                case "DS11v2":
                    familySku = "DSv2";
                    skuName = $"DS11 v2";
                    break;
                case "DS12v2":
                    familySku = "DSv2";
                    skuName = $"DS12 v2";
                    break;
                case "DS13v2":
                    familySku = "DSv2";
                    skuName = $"DS13 v2";
                    break;
                case "DS14v2":
                    familySku = "DSv2";
                    skuName = $"DS14 v2";
                    break;
                case "DS15v2":
                    familySku = "DSv2";
                    skuName = $"DS15 v2";
                    break;
                case "DC2asv5":
                    familySku = "DCasv5-";
                    skuName = $"Standard_DC2as_v5";
                    break;
                case "DC4asv5":
                    familySku = "DCasv5-";
                    skuName = $"Standard_DC4as_v5";
                    break;
                case "DC8asv5":
                    familySku = "DCasv5-";
                    skuName = $"Standard_DC8as_v5";
                    break;
                case "DC16asv5":
                    familySku = "DCasv5-";
                    skuName = $"Standard_DC16as_v5";
                    break;
                case "DC32asv5":
                    familySku = "DCasv5-";
                    skuName = $"Standard_DC32as_v5";
                    break;
                case "DC48asv5":
                    familySku = "DCasv5-";
                    skuName = $"Standard_DC48as_v5";
                    break;
                case "DC96asv5":
                    familySku = "DCasv5-";
                    skuName = $"Standard_DC96as_v5";
                    break;
                default:
                    this.logger.LogWarning("VM size {size} is not supported.", vmSize);
                    return null;
            }

            var prefix = "Virtual Machines ";
            if (familySku == "DCdsv3" || familySku == "DCsv3" || familySku == "DCadsv5-" || familySku == "DCasv5-")
            {
                prefix = string.Empty;
            }

            var postfix = " Series";
            if(familySku == "DCadsv5-" || familySku == "DCasv5-")
            {
                postfix = "series";
            }

            if(os == "Windows")
            {
                productName = $"{prefix}{familySku}{postfix} Windows";
            }
            else
            {
                productName = $"{prefix}{familySku}{postfix} Linux";
            }
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{skuName}' and productName eq '{productName}'";
    }
}
