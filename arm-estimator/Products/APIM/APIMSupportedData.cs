internal class APIMSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuIdMap = new Dictionary<string, string[]>()
    {
        {
            "Consumption", new[] 
            {
                "DZH318Z0BQCM/005P"
            }
        },
        {
            "Basic", new[]
            {
                "DZH318Z0BQCM/0023"
            }
        },
        {
            "Standard", new[]
            {
                "DZH318Z0BQCM/0029"
            }
        },
        {
            "Developer", new[]
            {
                "DZH318Z0BQCM/001K"
            }
        },
        {
            "Premium", new[]
            {
                "DZH318Z0BQCM/003D",
                "DZH318Z0BQCM/00HK"
            }
        },
        {
            "Isolated", new[]
            {
                ""
            }
        },
        {
            "Gateway", new[]
            {
                "DZH318Z0BQCM/00BM"
            }
        }
    };
}
