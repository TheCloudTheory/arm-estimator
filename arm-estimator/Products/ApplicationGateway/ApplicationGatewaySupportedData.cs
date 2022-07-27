internal class ApplicationGatewaySupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuIdMap = new Dictionary<string, string[]>()
    {
        {
            "Standard_Small", new[] 
            {
                "DZH318Z0BNXC/0037"
            }
        },
        {
            "Standard_Medium", new[]
            {
                "DZH318Z0BNXC/002R"
            }
        },
        {
            "Standard_Large", new[]
            {
                "DZH318Z0BNXC/000K"
            }
        },
        {
            "Standard_v2", new[]
            {
                "DZH318Z0BSD1/0004"
            }
        },
        {
            "WAF_Medium", new[]
            {
                "DZH318Z0BNVT/000V"
            }
        },
        {
            "WAF_Large", new[]
            {
                "DZH318Z0BNVT/0007"
            }
        },
        {
            "WAF_v2", new[]
            {
                "DZH318Z0BSD0/000B"
            }
        }
    };
}
