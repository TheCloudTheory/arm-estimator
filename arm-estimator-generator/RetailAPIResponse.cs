internal class RetailAPIResponse
{
    public RetailItem[]? Items { get; set; }
    public string? NextPageLink { get; set; }
}

internal class RetailItem
{
    public string? type { get; set; }
    public string? skuId { get; set; }
    public string? skuName { get; set; }
    public string? productId { get; set; }
    public string? productName { get; set; }
    public string? meterId { get; set; }
    public string? meterName { get; set; }
    public string? serviceName { get; set; }
    public string? serviceId { get; set; }
}