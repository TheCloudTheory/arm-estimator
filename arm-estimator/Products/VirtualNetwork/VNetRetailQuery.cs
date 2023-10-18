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

        if (properties.ContainsKey("virtualNetworkPeerings") && this.template.SpecialCaseResources != null)
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

            var peeringType = DecidePeeringType();

            if (peeringType == PeeringType.InterRegion)
            {
                return new RetailAPIResponse()
                {
                    Items = new[]
                    {
                        new RetailItem()
                        {
                            skuName = "Inter-Region",
                            productName = "Virtual Network Peering",
                            meterName = "Inbound data transfer",
                            retailPrice = 0.035d,
                            unitOfMeasure = "GB"
                        },
                        new RetailItem()
                        {
                            skuName = "Inter-Region",
                            productName = "Virtual Network Peering",
                            meterName = "Outbound data transfer",
                            retailPrice = 0.035d,
                            unitOfMeasure = "GB"
                        }
                    }
                };
            }

            if (peeringType == PeeringType.IntraRegion)
            {
                return new RetailAPIResponse()
                {
                    Items = new[]
                    {
                    new RetailItem()
                    {
                        skuName = "Intra-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Inbound data transfer",
                        retailPrice = 0.01d,
                        unitOfMeasure = "GB"
                    },
                    new RetailItem()
                    {
                        skuName = "Intra-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Outbound data transfer",
                        retailPrice = 0.01d,
                        unitOfMeasure = "GB"
                    }
                }
                };
            }

            return new RetailAPIResponse()
            {
                Items = new[]
                {
                   new RetailItem()
                   {
                       skuName = "Intra-Region",
                       productName = "Virtual Network Peering",
                       meterName = "Inbound data transfer",
                       retailPrice = 0.01d,
                       unitOfMeasure = "GB"
                   },
                   new RetailItem()
                   {
                       skuName = "Intra-Region",
                       productName = "Virtual Network Peering",
                       meterName = "Outbound data transfer",
                       retailPrice = 0.01d,
                       unitOfMeasure = "GB"
                   },
                   new RetailItem()
                    {
                        skuName = "Inter-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Inbound data transfer",
                        retailPrice = 0.035d,
                        unitOfMeasure = "GB"
                    },
                    new RetailItem()
                    {
                        skuName = "Inter-Region",
                        productName = "Virtual Network Peering",
                        meterName = "Outbound data transfer",
                        retailPrice = 0.035d,
                        unitOfMeasure = "GB"
                    }
               }
            };
        }

        return new RetailAPIResponse()
        {
            Items = Array.Empty<RetailItem>()
        };
    }

    private PeeringType DecidePeeringType()
    {
        var peeringTypes = new List<PeeringType>();
        var resourcesWithPeerings = this.template.SpecialCaseResources!.Where(resource => resource.Properties.VirtualNetworkPeerings != null);
        if(resourcesWithPeerings == null || resourcesWithPeerings.Any() == false)
        {
            throw new InvalidOperationException("Cannot decide peering type for the current configuration.");
        }

        foreach (var peering in resourcesWithPeerings)
        {
            var remoteVNet = base.changes.SingleOrDefault(c => peering.Properties.VirtualNetworkPeerings!.Any(peering => peering.Id == c.resourceId));
            if (remoteVNet == null)
            {
                this.logger.LogWarning("Couldn't determine remote VNet for peering. Assuming it's Inter-Region.");
                peeringTypes.Add(PeeringType.InterRegion);

                continue;
            }

            var remoteVNetId = new CommonResourceIdentifier(remoteVNet.resourceId!);
            if (remoteVNetId.GetLocation() == this.id.GetLocation())
            {
                peeringTypes.Add(PeeringType.IntraRegion);
                break;
            }
            else
            {
                peeringTypes.Add(PeeringType.InterRegion);
            }
        }

        return peeringTypes.Contains(PeeringType.IntraRegion) && peeringTypes.Contains(PeeringType.InterRegion) ?
            PeeringType.Both :
            peeringTypes.First();
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
