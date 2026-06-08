using ACE.WhatIf;
using Microsoft.Extensions.Logging;

internal class ContainerRegistryQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger) : IQueryFilter
{
    private const string ServiceId = "DZH315F9L8DM";

    private readonly WhatIfAfterBeforeChange afterState = afterState;
    private readonly ILogger logger = logger;

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var sku = this.afterState.sku?.name;
        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Container Registry when SKU is unavailable.");
            return null;
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}'";
    }
}
