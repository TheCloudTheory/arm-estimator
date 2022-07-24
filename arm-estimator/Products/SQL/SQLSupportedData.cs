internal class SQLSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuIdMap = new Dictionary<string, string[]>()
    {
        { "Free", new[] {
            "DZH318Z0BQH9/0013"
            }
        },
        { "Basic", new[] {
            "DZH318Z0BQGZ/004K"
            }
        },
        { "S0", new[] {
            "DZH318Z0BQHF/00LB"
            }
        },
        { "S1", new[] {
            "DZH318Z0BQHF/00LT"
            }
        },
        { "S2", new[] {
            "DZH318Z0BQHF/017X"
            }
        },
        { "S3", new[] {
            "DZH318Z0BQHF/00TX"
            }
        },
        { "S4", new[] {
            "DZH318Z0BQHF/014D"
            }
        },
        { "S6", new[] {
            "DZH318Z0BQHF/00TZ"
            }
        },
        { "S7", new[] {
            "DZH318Z0BQHF/012N"
            }
        },
        { "S9", new[] {
            "DZH318Z0BQHF/00SQ"
            }
        },
        { "S12", new[] {
            "DZH318Z0BQHF/00LJ"
            }
        }
    };

    public static readonly IReadOnlyDictionary<string, int> SkuToDTUsMap = new Dictionary<string, int>()
    {
        { "S4", 200 },
        { "S6", 400 },
        { "S7", 800 },
        { "S9", 1600 },
        { "S12", 3000 }
    };
}
