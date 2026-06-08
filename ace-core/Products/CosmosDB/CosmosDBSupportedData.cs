internal class CosmosDBSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuNameMap = new Dictionary<string, string[]>()
    {
        {
            "Account", new[] 
            {
                "RUs"
            }
        },
        {
            "Database", new[]
            {
                "RUs"
            }
        },
        {
            "Container", new[]
            {
                "RUs"
            }
        }
    };
}
