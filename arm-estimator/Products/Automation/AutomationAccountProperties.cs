using System.Text.Json.Serialization;

internal class AutomationAccountProperties
{
    [JsonPropertyName("sku")]
    public AutomationAccountSku? Sku { get; set; }
}

internal class AutomationAccountSku
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}