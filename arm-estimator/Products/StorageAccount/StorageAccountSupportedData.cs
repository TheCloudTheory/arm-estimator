internal class StorageAccountSupportedData
{
    public static readonly IReadOnlyDictionary<string, string> CommonSkuToSkuIdMap = new Dictionary<string, string>()
    {
        { "Standard_LRS", 
            "Standard LRS"
        },
        { "Standard_ZRS",
            "Standard LRS"
        },
        { "Standard_GRS",
            "Standard LRS"
        },
        { "Standard_GZRS",
            "Standard GRS"
        },
        { "Standard_RAGRS",
            "Standard RAGRS"
        },
        { "Standard_RAGZRS",
            "Standard RAGZRS"
        }
    };

}
