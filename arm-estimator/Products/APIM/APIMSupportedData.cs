internal class APIMSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuNameMap = new Dictionary<string, string[]>()
    {
        {
            "Consumption", new[] 
            {
                "Consumption"
            }
        },
        {
            "Basic", new[]
            {
                "Basic"
            }
        },
        {
            "Standard", new[]
            {
                "Standard"
            }
        },
        {
            "Developer", new[]
            {
                "Developer"
            }
        },
        {
            "Premium", new[]
            {
                "Premium",
                "Secondary"
            }
        },
        {
            "Isolated", new[]
            {
                "Isolated"
            }
        },
        {
            "Gateway", new[]
            {
                "Gateway"
            }
        }
    };
}
