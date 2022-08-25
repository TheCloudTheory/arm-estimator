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
            "Standard RA-GRS"
        },
        { "Standard_RAGZRS",
            "Standard RA-GZRS"
        },
        { "Premium_LRS",
            "Premium LRS"
        },
        { "Premium_ZRS",
            "Premium ZRS"
        }
    };

}
