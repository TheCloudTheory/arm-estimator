

namespace ACE.Products.VirtualNetwork;

internal sealed class VNetZones
{
    public static readonly string[] Zone1 = new string[] {
        "australiacentral",
        "australiacentral2",
        "canadacentral",
        "canadaeast",
        "centralus",
        "eastus",
        "eastus2",
        "francecentral",
        "francesouth",
        "germanynorth",
        "germanywestcentral",
        "northcentralus",
        "northeurope",
        "norwayeast",
        "norwaywest",
        "southcentralus",
        "switzerlandnorth",
        "switzerlandwest",
        "uksouth",
        "ukwest",
        "westcentralus",
        "westeurope",
        "westus",
        "westus2"
    };

    public static readonly string[] Zone2 = new string[] {
        "australiaeast",
        "australiasoutheast",
        "centralindia",
        "eastasia",
        "japaneast",
        "japanwest",
        "koreacentral",
        "koreasouth",
        "southeastasia",
        "southindia",
        "westindia",
    };

    public static readonly string[] Zone3 = new string[] {
        "brazilsouth",
        "southafricanorth",
        "southafricawest",
        "uaenorth",
        "uaecentral"
    };

    public static readonly double Zone1Cost = 0.035d;
    public static readonly double Zone2Cost = 0.09d;
    public static readonly double Zone3Cost = 0.16d;

    public static RetailItem[] GetCost(string inLocation, string outLocation)
    {
        return new[]
        {
            new RetailItem()
            {
                skuName = "Inter-Region",
                productName = "Virtual Network Peering",
                meterName = "Inbound data transfer",
                retailPrice = GetCost(inLocation),
                unitOfMeasure = "GB"
            },
            new RetailItem()
            {
                skuName = "Inter-Region",
                productName = "Virtual Network Peering",
                meterName = "Outbound data transfer",
                retailPrice = GetCost(outLocation),
                unitOfMeasure = "GB"
            }
        };
    }

    private static double GetCost(string location)
    {
        if (Zone1.Contains(location))
        {
            return Zone1Cost;
        }
        else if (Zone2.Contains(location))
        {
            return Zone2Cost;
        }
        else if (Zone3.Contains(location))
        {
            return Zone3Cost;
        }
        else
        {
            return 0.0;
        }
    }
}