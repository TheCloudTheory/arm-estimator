using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ACE.WhatIf;

internal class TerraformTemplateParser
{
    private readonly string template;
    private readonly ILogger<Program> logger;

    public TerraformTemplateParser(string template, ILogger<Program> logger)
    {
        this.template = template;
        this.logger = logger;
    }

    public WhatIfResponse? GetConfigurationAsWhatIfData(CancellationToken token)
    {
        if(token.IsCancellationRequested) return null;
        
        var data = JsonSerializer.Deserialize<TerraformData>(this.template);
        if (data == null)
        {
            this.logger.LogError("Cannot procees Terraform data.");
            return null;
        }

        var whatIf = new WhatIfResponse()
        {
            properties = new WhatIfProperties()
            {
                changes = data.Changes?.Select(c => DefineWhatIfChange(c, data)).ToArray()
            }
        };

        return whatIf;
    }

    private WhatIfChange DefineWhatIfChange(ResourceChange change, TerraformData data)
    {
        if (change.Change == null)
        {
            this.logger.LogError("Couldn't procees data for {type}.{name}", change.Type, change.Name);
            return new WhatIfChange();
        }

        return new WhatIfChange()
        {
            changeType = DetermineChangeType(change),
            resourceId = $"terraform{DetermineParent(change, data)}.{change.Type}.{change.Name}.{GetLocation(change)}.{change.Change?.After!["name"]}",
            after = new WhatIfAfterBeforeChange()
            {
                type = change.Type,
                location = GetLocation(change),
                properties = change.Change?.After,
                sku = CreateSkuObjectIfProvided(change.Change)
            }
        };
    }

    private static string? DetermineParent(ResourceChange change, TerraformData data)
    {
        var parentRef = data.Configuration?.RootModule?.Resources?.FirstOrDefault(_ => _.Name == change.Name && _.Type == change.Type)?.Expressions?.ResourceGroup?.References?[1];
        if(parentRef == null)
        {
            return null;
        }

        var refParts = parentRef.Split('.');
        var parent = data.Changes?.FirstOrDefault(_ => _.Name == refParts[1] && _.Type == refParts[0]);
        if(parent == null)
        {
            return null;
        }

        return $".{parent.Type}.{parent.Name}.{GetLocation(parent)}.{parent.Change?.After!["name"]}";
    }

    private static string? GetLocation(ResourceChange change)
    {
        if(change.Change != null && change.Change.After != null && change.Change.After.ContainsKey("location"))
        {
            return change.Change?.After!["location"].ToString();
        }

        return null;
    }

    /// <summary>
    /// Maps Terraform action to What If change type for compatibility
    /// </summary>
    private static WhatIfChangeType? DetermineChangeType(ResourceChange change)
    {
        if (change.Change == null) return null;
        if (change.Change.Actions == null) return null;

        var action = change.Change.Actions.FirstOrDefault();
        if (action == null) return null;

        if (action == "create") return WhatIfChangeType.Create;
        return null;
    }

    private WhatIfSku? CreateSkuObjectIfProvided(ResourceChangeData? data)
    {
        if(data == null || data.After == null)
        {
            return null;
        }

        if(data.After.ContainsKey("sku") == false)
        {
            // We may need to fall back to other ways of determining SKU of a resource.
            // Unfortunately TF uses different ways of describing it, so we need to do
            // that in per-resource manner.
            if(data.After.ContainsKey("account_tier") && data.After.ContainsKey("account_replication_type"))
            {
                return new WhatIfSku()
                {
                    name = $"{data.After["account_tier"]}_{data.After["account_replication_type"]}"
                };
            }

            return null;
        }

        var skuData = (JsonElement)data.After["sku"];

        // For some resources, SKU is represented as Array containing single
        // SKU object. As calculation of some resources (like AppGW) assumes,
        // that SKU is an object (thus it's being deserialized to IDictionary<string, object>),
        // we need to extract SKU value from the array.
        if(skuData.ValueKind == JsonValueKind.Array)
        {
            var sku = skuData[0].Deserialize<WhatIfSku>();
            return sku;
        }

        return new WhatIfSku()
        {
            name = skuData.ToString()
        };
    }
}
