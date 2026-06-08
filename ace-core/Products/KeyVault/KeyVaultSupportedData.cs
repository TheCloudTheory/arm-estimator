    internal class KeyVaultSupportedData
{
    public static readonly IReadOnlyDictionary<string, string> SkuToSkuNameMap = new Dictionary<string, string>()
    {
        {
            "standard",
                "Standard"
        },
        {
            "premium",
                "Premium"
        },
        {
            "Standard_B1",
                "Standard B1"
        }
    };

    public static readonly IReadOnlyDictionary<string, string> SkuToProductNameMap = new Dictionary<string, string>()
    {
        {
            "standard",
                "Key Vault"
        },
        {
            "premium",
                "Key Vault"
        },
        {
            "Standard_B1",
                "Key Vault HSM Pool"
        }
    };
}
