using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ACE.Products.VirtualNetwork;

internal class VNetRetailQuery : BaseRetailQuery, IRetailQuery
{
    public VNetRetailQuery(WhatIfChange change,
                           CommonResourceIdentifier id,
                           ILogger logger,
                           CurrencyCode currency,
                           WhatIfChange[] changes,
                           TemplateSchema template)
        : base(change, id, logger, currency, changes, template)
    {
    }

    public RetailAPIResponse? GetFakeResponse()
    {
        if (this.change.after == null && this.change.before == null)
        {
            this.logger.LogError("Can't generate Retail API query if desired state is unavailable.");
            return null;
        }

        var change = this.change.after ?? this.change.before;
        if (change == null)
        {
            this.logger.LogError("Couldn't determine after / before state.");
            return null;
        }

        var properties = change.properties;
        if (properties == null)
        {
            this.logger.LogWarning("Cannot build filter for Virtual Network if properies are not available.");
            return null;
        }

        if (properties.ContainsKey("virtualNetworkPeerings"))
        {
            var peerings = ((JsonElement)properties["virtualNetworkPeerings"]!).Deserialize<Peering[]>();
            if (peerings == null)
            {
                this.logger.LogError("Couldn't deserialize VNet peerings");
                return null;
            }

            if (this.currency != CurrencyCode.USD)
            {
                this.logger.LogWarning("ACE currently doesn't support calculating VNet peerings cost for other currency than USD. Make appropriate adjustment to reflect real cost of this service.");
            }

            var peeringTypes = DetectPeeringTypes(peerings);
            var items = new List<RetailItem>();
            foreach (var peeringType in peeringTypes)
            {
                if (peeringType == PeeringType.InterRegion)
                {
                    items.Add(new RetailItem()
                    {
                        skuName = "Inter-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Inbound data transfer",
                        retailPrice = 0.035d,
                        unitOfMeasure = "GB"
                    });

                    items.Add(new RetailItem()
                    {
                        skuName = "Inter-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Outbound data transfer",
                        retailPrice = 0.035d,
                        unitOfMeasure = "GB"
                    });
                }

                if (peeringType == PeeringType.IntraRegion)
                {
                    items.Add(new RetailItem()
                    {
                        skuName = "Intra-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Inbound data transfer",
                        retailPrice = 0.01d,
                        unitOfMeasure = "GB"
                    });

                    items.Add(new RetailItem()
                    {
                        skuName = "Intra-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Outbound data transfer",
                        retailPrice = 0.01d,
                        unitOfMeasure = "GB"
                    });
                }

                if (peeringType == PeeringType.Both)
                {
                    items.Add(new RetailItem()
                    {
                        skuName = "Intra-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Inbound data transfer",
                        retailPrice = 0.01d,
                        unitOfMeasure = "GB"
                    });

                    items.Add(new RetailItem()
                    {
                        skuName = "Intra-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Outbound data transfer",
                        retailPrice = 0.01d,
                        unitOfMeasure = "GB"
                    });

                    items.Add(new RetailItem()
                    {
                        skuName = "Inter-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Inbound data transfer",
                        retailPrice = 0.035d,
                        unitOfMeasure = "GB"
                    });

                    items.Add(new RetailItem()
                    {
                        skuName = "Inter-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Outbound data transfer",
                        retailPrice = 0.035d,
                        unitOfMeasure = "GB"
                    });
                }
            }

            return new RetailAPIResponse()
            {
                Items = items.ToArray()
            };
        }

        return new RetailAPIResponse()
        {
            Items = Array.Empty<RetailItem>()
        };
    }

    private PeeringType[] DetectPeeringTypes(Peering[] peerings)
    {
        var peeringTypes = new List<PeeringType>();

        foreach (var peering in peerings)
        {
            var remoteVNet = base.changes.SingleOrDefault(c => peering.Properties != null && peering.Properties.RemoteVirtualNetwork != null && peering.Properties.RemoteVirtualNetwork.Id == c.resourceId);
            if (remoteVNet == null)
            {
                this.logger.LogWarning("Couldn't determine remote VNet for peering. Assuming it's Inter-Region.");
                peeringTypes.Add(PeeringType.InterRegion);

                continue;
            }

            if (remoteVNet.after?.location == this.change.after?.location)
            {
                peeringTypes.Add(PeeringType.IntraRegion);
                break;
            }
            else
            {
                peeringTypes.Add(PeeringType.InterRegion);
            }
        }

        var resourcesWithPeerings = this.template.SpecialCaseResources!.Where(resource => resource.Properties?.VirtualNetworkPeerings != null);
        if (resourcesWithPeerings != null)
        {
            foreach (var peering in resourcesWithPeerings)
            {
                var remoteVNet = base.changes.SingleOrDefault(c => peering.Properties != null && peering.Properties.VirtualNetworkPeerings!.Any(peering => peering.Id == c.resourceId));
                if (remoteVNet == null)
                {
                    this.logger.LogWarning("Couldn't determine remote VNet for peering. Assuming it's Inter-Region.");
                    peeringTypes.Add(PeeringType.InterRegion);

                    continue;
                }

                if (remoteVNet.after?.location == this.change.after?.location)
                {
                    peeringTypes.Add(PeeringType.IntraRegion);
                    break;
                }
                else
                {
                    peeringTypes.Add(PeeringType.InterRegion);
                }
            }
        }

        return peeringTypes.ToArray();
    }

    public string? GetQueryUrl(string location)
    {
        if (this.change.after == null && this.change.before == null)
        {
            this.logger.LogError("Can't generate Retail API query if desired state is unavailable.");
            return null;
        }

        var change = this.change.after == null ? this.change.before : this.change.after;
        if (change == null)
        {
            this.logger.LogError("Couldn't determine after / before state.");
            return null;
        }

        var filter = new VNetQueryFilter(change, this.logger).GetFiltersBasedOnDesiredState(location);
        return $"{baseQuery}{filter}";
    }
}
