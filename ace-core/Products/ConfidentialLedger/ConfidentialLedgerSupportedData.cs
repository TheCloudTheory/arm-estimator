internal class ConfidentialLedgerSupportedData
{
    public static readonly IReadOnlyDictionary<string, string[]> SkuToSkuNameMap = new Dictionary<string, string[]>()
    {
        {
            "Ledger", new[] 
            {
                "Storage",
                "Ledger P1"
            }
        }
    };
}
