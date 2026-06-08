internal class RetailAPIResponse
{
    public RetailItem[]? Items { get; set; }
    public string? Url { get; set; }
}

/// <summary>A single pricing record returned by the Azure Retail Prices API.</summary>
public class RetailItem
{
    public double? retailPrice { get; set; }
    public string? type { get; set; }
    public string? skuId { get; set; }
    public string? skuName { get; set; }
    public string? productId { get; set; }
    public string? productName { get; set; }
    public string? meterId { get; set; }
    public string? meterName { get; set; }
    public string? unitOfMeasure { get; set; }
    public string? location { get; set; }
    public string? serviceId { get; set; }
}
