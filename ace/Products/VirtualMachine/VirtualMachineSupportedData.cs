internal class VirtualMachineSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuIdMap = new Dictionary<string, string[]>()
    {
        {
            "Basic_A_Windows", new[] 
            {
                "A0",
                "A1",
                "A2",
                "A3",
            }
        },
        {
            "Basic_A_Linux", new[]
            {
                "A0",
                "A1",
                "A2",
                "A3",
            }
        }
    };
}
