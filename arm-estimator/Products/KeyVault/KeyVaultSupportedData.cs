internal class KeyVaultSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuIdMap = new Dictionary<string, string[]>()
    {
        {
            "standard", new[] 
            {
                "DZH318Z0BQG0/003C"
            }
        },
        {
            "premium", new[]
            {
                "DZH318Z0BQG0/001Z"
            }
        },
        {
            "Standard_B1", new[]
            {
                "DZH318Z0DHT9/000Z"
            }
        },
        {
            "DZH318Z0BQF5/0004", new[]
            {
                "DZH318Z0CHL0/0005"
            }
        }
    };
}
