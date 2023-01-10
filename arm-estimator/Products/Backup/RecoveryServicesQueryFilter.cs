using Azure.Core;
using Microsoft.Extensions.Logging;
using System.Linq;

internal class RecoveryServicesQueryFilter : IQueryFilter
{
    private const string ServiceId = "DZH3155G7B5D";

    private readonly WhatIfAfterBeforeChange afterState;
    private readonly ILogger logger;
    private readonly WhatIfChange[] changes;
    private readonly ResourceIdentifier id;

    public RecoveryServicesQueryFilter(WhatIfAfterBeforeChange afterState,
                                       ILogger logger,
                                       WhatIfChange[] changes,
                                       ResourceIdentifier id)
    {
        this.afterState = afterState;
        this.logger = logger;
        this.changes = changes;
        this.id = id;
    }

    public string? GetFiltersBasedOnDesiredState(string location)
    {
        var meterName = "GRS Data Stored";
        var backupConfiguration = this.changes.SingleOrDefault(_ => GetAfterOrBefore(_)?.type == "Microsoft.RecoveryServices/vaults/backupstorageconfig" && _.resourceId != null && _.resourceId.Contains(this.id.Name));
        if (backupConfiguration != null)
        {
            var change = GetAfterOrBefore(backupConfiguration);
            if(change != null)
            {
                if(change.properties != null && change.properties.ContainsKey("storageModelType"))
                {
                    var rendundancyMode = change.properties["storageModelType"]?.ToString();
                    switch(rendundancyMode)
                    {
                        case "GeoRedundant":
                            meterName = "GRS Data Stored";
                            break;
                        case "LocallyRedundant":
                            meterName = "LRS Data Stored";
                            break;
                        case "ZoneRedundant":
                            meterName = "ZRS Data Stored";
                            break;
                        case "ReadAccessGeoZoneRedundant":
                            meterName = "RA-GRS Data Stored";
                            break;
                    }
                }
            }
        }

        return $"serviceId eq '{ServiceId}' and armRegionName eq '{location}' and skuName eq 'Standard' and meterName eq '{meterName}'";
    }

    private WhatIfAfterBeforeChange? GetAfterOrBefore(WhatIfChange change)
    {
        var result = change.after ?? change.before;
        if(result == null)
        {
            this.logger.LogError("Couldn't determine WhatIf state as both after and before states are null.");
            return null;
        }

        return result;
    }
}
