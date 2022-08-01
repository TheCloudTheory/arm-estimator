using Microsoft.Extensions.Logging;

internal class CosmosDBQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH317DL9CC7";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public CosmosDBQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        string? sku = null;
        var type = this.afterState.type;

        if(type != null && type == "Microsoft.DocumentDB/databaseAccounts")
        {
            sku = "Account";
        }

        if (type != null && type == "Microsoft.DocumentDB/databaseAccounts/sqlDatabases")
        {
            sku = "Database";
        }

        if (type != null && type == "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers")
        {
            sku = "Container";
        }

        if (sku == null)
        {
            this.logger.LogError("Can't create a filter for Cosmos DB when SKU is unavailable.");
            return null;
        }

        var skuIds = CosmosDBSupportedData.SkuToSkuIdMap[sku];
        var skuIdsFilter = string.Join(" or ", skuIds.Select(_ => $"skuId eq '{_}'"));

        return $"$filter=serviceId eq '{ServiceId}' and armRegionName eq '{location}' and ({skuIdsFilter})";
    }
}
