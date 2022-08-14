internal class VirtualMachineSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuIdMap = new Dictionary<string, string[]>()
    {
        {
            "Basic_A0_Windows", new[] 
            {
                "DZH318Z0BQNM/00D0"
            }
        },
        {
            "Basic_A1_Windows", new[]
            {
                "DZH318Z0BQNM/001G"
            }
        },
        {
            "Basic_A2_Windows", new[]
            {
                "DZH318Z0BQNM/0087"
            }
        },
        {
            "Basic_A3_Windows", new[]
            {
                "DZH318Z0BQNM/0067"
            }
        },
        {
            "Basic_A4_Windows", new[]
            {
                "DZH318Z0BQNM/000M"
            }
        }
    };
}
