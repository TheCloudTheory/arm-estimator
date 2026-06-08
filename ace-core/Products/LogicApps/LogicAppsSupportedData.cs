internal class LogicAppsSupportedData
{
    public static readonly IReadOnlyDictionary<string, string> SkuToSkuNameMap = new Dictionary<string, string>()
    {
        {
            "Consumption",
                "Consumption"
        },
        {
            "Free",
                ""
        },
        {
            "Basic", 
                "Base"
        },
        {
            "Standard",
                "Base"
        }
    };

    public static readonly IReadOnlyDictionary<string, string> SkuToProductNameMap = new Dictionary<string, string>()
    {
        {
            "Consumption",
                "Logic Apps"
        },
        {
            "Free",
                ""
        },
        {
            "Basic",
                "Logic Apps Integration Service Environment - Developer"
        },
        {
            "Standard",
                "Logic Apps"
        }
    };
}
