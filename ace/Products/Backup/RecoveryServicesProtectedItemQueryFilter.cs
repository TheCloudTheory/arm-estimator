using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class RecoveryServicesProtectedItemQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3155G7B5D";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;

    public RecoveryServicesProtectedItemQueryFilter(WhatIfAfterBeforeChange afterState, ILogger logger)
    {
        this.afterState = afterState;
        this.logger = logger;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var propertiesRaw = JsonSerializer.Serialize(this.afterState.properties);
        var properties = JsonSerializer.Deserialize<ProtectedItemProperties>(propertiesRaw);

        if(properties?.protectedItemType == null)
        {
            this.logger.LogError("Can't create a filter for Protected Item when its type is unavailable.");
            return null;
        }

        string? sku;
        switch(properties.protectedItemType)
        {
            case "AzureVmWorkloadSQLDatabase":
                sku = "SQL Server in Azure VM";
                break;
            case "Microsoft.Compute/virtualMachines":
                sku = "Azure VM";
                break; 
            case "AzureVmWorkloadSAPHanaDatabase":
            case "AzureVmWorkloadSAPAseDatabase":
                sku = "SAP HANA on Azure VM";
                break;
            case "AzureFileShareProtectedItem":
                sku = "Azure Files";
                break;
            default:
                this.logger.LogWarning("Protected item type {type} is not yet supported.", properties.protectedItemType);
                return null;
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq '{sku}'";
    }
}
