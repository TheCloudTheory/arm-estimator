using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class ServiceBusQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3179N6FXW";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public ServiceBusQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Service Bus when SKU is unavailable.");
            return null;
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}'";
    }
}
