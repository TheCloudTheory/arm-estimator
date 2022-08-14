internal class StorageAccountSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> CommonSkuToSkuIdMap = new Dictionary<string, string[]>()
    {
        { "Standard_LRS", new[] {
            "DZH318Z0BNZ5/0056",
            "DZH318Z0BNZ4/003N"
            }
        },
        { "Standard_GRS", new[] {
            "DZH318Z0BNZH/004T"
            }
        },
        { "Standard_RAGRS", new[] {
            ""
            }
        }
    };

    public static readonly string[] HotTierSkuIds = new[]
    {
        "DZH318Z0BNZH/004V"
    };

    public static readonly string[] CoolTierSkuIds = new[]
    {
        "DZH318Z0BPH7/00BZ"
    };
}
