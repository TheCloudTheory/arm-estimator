internal class ApplicationInsightsSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuNameMap = new Dictionary<string, string[]>()
    {
        {
            "classic", new[] 
            {
                "Pay-as-you-go",
                "Basic"
            }
        }
    };
}
