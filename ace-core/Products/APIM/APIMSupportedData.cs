internal class APIMSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuNameMap = new Dictionary<string, string[]>()
    {
        {
            "Consumption", [
                "Consumption"
            ]
        },
        {
            "Basic", [
                "Basic"
            ]
        },
        {
            "Standard", [
                "Standard"
            ]
        },
        {
            "Developer", [
                "Developer"
            ]
        },
        {
            "Premium", [
                "Premium",
                "Secondary"
            ]
        },
        {
            "Isolated", [
                "Isolated"
            ]
        },
        {
            "Gateway", [
                "Gateway"
            ]
        }
    };
}
