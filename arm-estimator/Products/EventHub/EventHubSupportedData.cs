internal class EventHubSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuIdMap = new Dictionary<string, string[]>()
    {
        {
            "Basic", new[] 
            {
                "DZH318Z0BQFF/003M"
            }
        },
        {
            "Standard", new[]
            {
                "DZH318Z0BQFF/003N"
            }
        },
        {
            "Premium", new[]
            {
                "DZH318Z0BQFF/009V"
            }
        },
        {
            "Dedicated", new[]
            {
                "DZH318Z0BQFF/003P"
            }
        },
        {
            "Capture", new[]
            {
                "DZH318Z0BQFF/003N"
            }
        },
        {
            "Empty", new[]
            {
                ""
            }
        }
    };
}
