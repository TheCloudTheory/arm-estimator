using System.Text.Json;
using ACE.Extensions;
using ACE.WhatIf;
using Microsoft.Extensions.Logging;

namespace ACE.Products.ASR;

internal class AzureSiteRecoveryRetailQuery : BaseRetailQuery, IRetailQuery
{
    public AzureSiteRecoveryRetailQuery(WhatIfChange change, CommonResourceIdentifier id, ILogger logger, CurrencyCode currency, WhatIfChange[] changes, TemplateSchema template) : base(change, id, logger, currency, changes, template)
    {
    }

    public RetailAPIResponse? GetFakeResponse()
    {
        throw new NotImplementedException();
    }

    public string? GetQueryUrl(string location)
    {
        if (this.change.after == null && this.change.before == null)
        {
            this.logger.LogError("Can't generate Retail API query if desired state is unavailable.");
            return null;
        }

        var change = this.change.GetChange();
        if (change == null)
        {
            this.logger.LogError("Couldn't determine after / before state.");
            return null;
        }

        var filter = new AzureSiteRecoveryQueryFilter(change, this.logger).GetFiltersBasedOnDesiredState(location);
        var protectedItem = this.changes.FirstOrDefault(change => change.GetChange()!.type == "Microsoft.RecoveryServices/vaults/replicationFabrics/replicationProtectionContainers/replicationProtectedItems");

        if (protectedItem != null)
        {
            var properties = protectedItem.GetChange()!.properties;
            if (properties != null)
            {
                var protectedVm = JsonSerializer.Deserialize<ReplicationProtectedItemProperties>(JsonSerializer.Serialize(properties));
                if (protectedVm != null)
                {
                    var fabricObjectId = protectedVm.ProviderSpecificDetails.FabricObjectId;
                    var instanceType = protectedVm.ProviderSpecificDetails.InstanceType;
                    if (fabricObjectId != null && instanceType == "A2A")
                    {
                        var vm = this.changes.FirstOrDefault(change => change.resourceId == fabricObjectId);
                        if (vm == null)
                        {
                            this.logger.LogWarning("Couldn't find VM for A2A protected item. Looked for {fabricObjectId}. ACE is unable to calculate inferred metrics for replicated VM.", fabricObjectId);
                        }
                        else
                        {
                            // If we have information about replicated VM, we can calculate inferred metrics.
                            // To do so, we need to find where the VM is going to be replicated to.
                            // The logic to find the target is as follows:
                            // - replicationProtectionContainerMappings have a pointer to replicationProtectionContainers
                            // - replicationProtectionContainers have a pointer to replicationFabrics via parent
                            // 
                            // The tricky part is finding replicationFabrics without direct link.
                            var replicationFabricIdentifier = new CommonResourceIdentifier(protectedItem.resourceId!).GetParent()!.GetParent();
                            if (replicationFabricIdentifier == null)
                            {
                                this.logger.LogWarning("Couldn't find identifier for replicationFabric required for inferring metrics.");
                            }
                            else
                            {
                                var replicationFabricId = replicationFabricIdentifier.ToString();
                                var replicationFabric = this.changes.FirstOrDefault(change => change.resourceId == replicationFabricId)?.GetChange();
                                if (replicationFabric == null)
                                {
                                    this.logger.LogWarning("Couldn't find identifier for replicationFabric required for inferring metrics.");
                                }
                                else
                                {
                                    var replicationFabricProperties = JsonSerializer.Deserialize<ReplicationFabricProperties>(JsonSerializer.Serialize(replicationFabric.properties));
                                    if (replicationFabricProperties != null)
                                    {
                                        if (replicationFabricProperties.CustomDetails.InstanceType == "Azure" && replicationFabricProperties.CustomDetails.Location != null)
                                        {
                                            var replicaLocation = replicationFabricProperties.CustomDetails.Location;

                                            // We need to update the location of the VM to the location of the replica.
                                            // This is a simple hack so we don't need to rebuild the whole WhatIfChange object.
                                            var vmChange = vm.GetChange()!;
                                            vmChange.location = replicaLocation;

                                            var replicaFilter = new VirtualMachineQueryFilter(vmChange, logger).GetFiltersBasedOnDesiredState(replicaLocation);
                                            return $"{baseQuery}({filter}) or (priceType eq 'Consumption' and {replicaFilter})";
                                        }
                                        else
                                        {
                                            this.logger.LogWarning("Couldn't find location for replicationFabric required for inferring metrics.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return $"{baseQuery}{filter}";
    }
}
