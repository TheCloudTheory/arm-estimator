using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class EventHubQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH318TR2LMQ";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public EventHubQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var type = this.afterState.type;
        if(type == null)
        {
            this.logger.LogError("Can't create a filter for Event Hub when type is unavailable.");
            return null;
        }

        var sku = this.afterState.sku?.name;
        if (type == "Microsoft.EventHub/namespaces/eventhubs")
        {
            sku = "Empty";
            var properties = this.afterState.properties;
            if(properties != null && properties.ContainsKey("captureDescription"))
            {
                var capture = ((JsonElement)properties["captureDescription"]).Deserialize<Dictionary<string, object>>();
                if(capture != null && capture.ContainsKey("enabled"))
                {
                    var captureValue = capture["enabled"].ToString() ?? "false";
                    var isCaptureEnabled = bool.Parse(captureValue);

                    if (isCaptureEnabled) sku = "Capture";
                }
            }
        }
        
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Event Hub when SKU is unavailable.");
            return null;
        }

        var skuIds = EventHubSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        if (sku == "Capture")
        {
            return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter}) and meterId eq '36085934-4216-4d15-a257-9670b5eb12dc'";
        }

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
