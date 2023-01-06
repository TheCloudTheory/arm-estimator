using Azure.Core;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class VNetRetailQuery : BaseRetailQuery, IRetailQuery
{
    public VNetRetailQuery(WhatIfChange change, ResourceIdentifier id, ILogger logger, CurrencyCode currency)
        : base(change, id, logger, currency)
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
            var peerings = ((JsonElement)properties["virtualNetworkPeerings"]).Deserialize<Peering[]>();
            if (peerings == null)
            {
                this.logger.LogError("Couldn't deserialize VNet peerings");
                return null;
            }

            if(this.currency != CurrencyCode.USD)
            {
                this.logger.LogWarning("ACE currently doesn't support calculating VNet peerings cost for other currency than USD. Make appropriate adjustment to reflect real cost of this service.");
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
                   }
               }
            };
        }

        return new RetailAPIResponse()
        {
            Items = Array.Empty<RetailItem>()
        };
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
