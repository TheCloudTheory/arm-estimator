internal class ApplicationGatewaySupportedData
{
    public static readonly IReadOnlyDictionary<string, string> SkuToSkuNameMap = new Dictionary<string, string>()
    {
        {
            "Standard_Small",
                "Small"
        },
        {
            "Standard_Medium",
                "Medium"
        },
        {
            "Standard_Large",
                "Large"
        },
        {
            "Standard_v2",
                "Standard"
        },
        {
            "WAF_Medium",
                "Medium"
        },
        {
            "WAF_Large",
                "Large"
        },
        {
            "WAF_v2",
                "Standard"
        }
    };

    public static readonly IReadOnlyDictionary<string, string[]> SkuToProductNameMap = new Dictionary<string, string[]>()
    {
        {
            "Standard_Small", new[]
            {
                "Basic Application Gateway"
            }
        },
        {
            "Standard_Medium", new[]
            {
                "Basic Application Gateway"
            }
        },
        {
            "Standard_Large", new[]
            {
                "Basic Application Gateway"
            }
        },
        {
            "Standard_v2", new[]
            {
                "Application Gateway Standard v2"
            }
        },
        {
            "WAF_Medium", new[]
            {
                "WAF Application Gateway"
            }
        },
        {
            "WAF_Large", new[]
            {
                "WAF Application Gateway"
            }
        },
        {
            "WAF_v2", new[]
            {
                "Application Gateway WAF v2"
            }
        }
    };
}
