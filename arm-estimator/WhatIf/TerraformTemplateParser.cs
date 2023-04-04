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

    public WhatIfResponse? GetConfigurationAsWhatIfData()
    {
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
                changes = data.Changes?.Select(DefineWhatIfChange).ToArray()
            }
        };

        return whatIf;
    }

    private WhatIfChange? DefineWhatIfChange(ResourceChange change)
    {
        if(change.Change == null)
        {
            this.logger.LogError($"Couldn't procees data for {change.Type}.{change.Name}");
            return null;
        }

        return new WhatIfChange()
        {
            changeType = DetermineChangeType(change),
            resourceId = $"terraform.{change.Type}.{change.Name}.{change.Change?.After!["location"]}.{change.Change?.After!["name"]}",
            after = new WhatIfAfterBeforeChange()
            {
                type = change.Type,
                location = change.Change?.After!["location"].ToString(),
                properties = change.Change?.After,
                sku = CreateSkuObjectIfProvided(change.Change)
            }
        };
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
            return null;
        }

        var skuData = data.After["sku"];
        return new WhatIfSku()
        {
            name = skuData.ToString()
        };
    }
}
