using ACE.Template;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.Json;

internal class TemplateParser
{
    public TemplateSchema Template { get; private set; }
    public ParametersSchema? Parameters => this.parameters;

    private readonly ParametersSchema? parameters;
    private readonly IEnumerable<string>? inlineParameters;
    private readonly string scopeId;
    private readonly string? resourceGroupName;
    private readonly ILogger logger;

    public TemplateParser(
        string template, 
        string parameters, 
        IEnumerable<string>? inlineParameters, 
        string scopeId, 
        string? resourceGroupName, 
        bool isUsingBicepparamFile,
        ILogger logger)
    {
        var t = JsonSerializer.Deserialize<TemplateSchema>(template) ?? throw new InvalidOperationException("Couldn't parse given template.");
        Template = t;

        if(isUsingBicepparamFile)
        {
            var bicepparamSchema = JsonSerializer.Deserialize<BicepparamParametersSchema>(parameters);
            this.parameters = JsonSerializer.Deserialize<ParametersSchema>(bicepparamSchema?.ParametersJson ?? "{}");
        }
        else
        {
            this.parameters = JsonSerializer.Deserialize<ParametersSchema>(parameters);
        }

        this.inlineParameters = inlineParameters;
        this.scopeId = scopeId;
        this.resourceGroupName = resourceGroupName;
        this.logger = logger;
    }

    internal void MaterializeFunctionsInsideTemplate(CancellationToken token)
    {
        if(token.IsCancellationRequested) return;
        if(Template.SpecialCaseResources == null) return;

        foreach(var specialCaseResource in Template.SpecialCaseResources)
        {
            if(specialCaseResource.Properties == null) continue;
            if(specialCaseResource.Properties.VirtualNetworkPeerings != null)
            {
                foreach(var peering in specialCaseResource.Properties.VirtualNetworkPeerings)
                {
                    if (peering == null || peering.Id == null) continue;

                    var peeringId = peering.Id.Replace("[", string.Empty);
                    peeringId = peeringId.Replace("]", string.Empty);

                    if(peeringId.StartsWith("resourceId"))
                    {
                        var resourceId = ParseResourceIdFunction(peeringId);
                        peering.Id = resourceId;
                    }
                }
            }
        }
    }

    private string ParseResourceIdFunction(string value)
    {
        // Remove unnecessary characters from the string, so we end up with 
        // something like 'Microsoft.Network/virtualNetworks', parameters('vnetName2')
        value = value.Replace("resourceId(", string.Empty);
        value = value[..^1];

        var parts = value.Split(',');
        var resourceType = parts[0].Replace("'", string.Empty);
        if (parts.Length == 2)
        {
            // If we have two elements in an array, it means that resourceId() is performed for the same
            // resource group as deployment. This is the simplest scenario, but we still need to extend
            // the identifier fully as resourceId() would so the rest of the code can make correct
            // assumptions
            var resourceName = ParseResourceName(parts[1]);
            return $"/subscriptions/{this.scopeId}/resourceGroups/{this.resourceGroupName}/providers/{resourceType}/{resourceName}";
        }

        this.logger.LogWarning("ACE doesn't support parsing resourceId() if resource outside of template.");
        return $"/subscriptions/{this.scopeId}/resourceGroups/providers/{this.resourceGroupName}NOT_SUPPORTED";
    }

    private string ParseResourceName(string value)
    {
        var name = value.Trim();
        if(name.StartsWith("parameters("))
        {
            name = name.Replace("parameters(", string.Empty);
            name = name[..^1];
            name = name.Replace("'", string.Empty);

            // Now we have name of a parameter which can be used to extract its value
            if(this.parameters == null || this.parameters.Parameters == null)
            {
                this.logger.LogError("ACE tried to parse identifier of a resource for non-existing parameters. This indicates bug or invalid state of an object processed.");
                return string.Empty;
            }

            return this.parameters.Parameters[name].Value!.ToString()!;
        }

        return name;
    }

    public void ParseParametersAndMaterializeFunctions(out string parameters, CancellationToken token)
    {
        if(token.IsCancellationRequested)
        {
            parameters = "{}";
            return;
        }

        if (this.parameters != null)
        {
            if(this.Template == null)
            {
                this.logger.LogError("Couldn't parse template.");
                parameters = "{}";
                return;
            }

            if(this.inlineParameters == null)
            {
                parameters = "{}";
                return;
            }

            foreach (var param in this.inlineParameters)
            {
                var keyValue = param.Split(new[] { '=' }, 2);
                if (keyValue.Length < 2)
                {
                    this.logger.LogError("Couldn't parse {param} as inline parameter.", param);
                    parameters = "{}";
                    return;
                }

                var parameterName = keyValue[0];
                var parameterValue = keyValue[1];
                var value = ParseParamaterByTheirType(parameterName, parameterValue);

                this.parameters.Parameters ??= new Dictionary<string, Parameter>();
                if (this.parameters.Parameters.ContainsKey(parameterName))
                {
                    this.parameters.Parameters[parameterName].Value = value;
                }
                else
                {
                    this.parameters.Parameters.Add(parameterName, new Parameter() { Value = value });
                }
            }

            this.MaterializeFunctionsInsideTemplate(token);

            parameters = JsonSerializer.Serialize(this.parameters, new JsonSerializerOptions()
            {
                WriteIndented = false,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        } 
        else
        {
            parameters = "{}";
        }
    }

    private object ParseParamaterByTheirType(string name, string value)
    {
        if(this.Template!.Parameters == null)
        {
            throw new TemplateParsingException("Parameters cannot be null when parsed.");
        }

        if(this.Template!.Parameters.TryGetValue(name, out var parameterSchema) == false)
        {
            throw new TemplateParsingException($"Couldn't parse {name} as parameter.");
        }

        if(parameterSchema.Type == null)
        {
            throw new TemplateParsingException($"Couldn't parse {name} as parameter - missing type.");
        }

        switch(parameterSchema.Type)
        {
            case "string":
                // If a user passes a parameter as empty string (by using '' or "", eg. myparam=''),
                // ACE needs to properly pass it as empty value.
                // See https://github.com/TheCloudTheory/arm-estimator/issues/118
                if (value == "''" || value == "\"\"")
                {
                    return string.Empty;
                }

                return value;
            case "bool":
                return bool.Parse(value);
            case "int":
                return int.Parse(value);
            case "object":
                return JsonSerializer.Deserialize(value, typeof(object))!;
            case "secureObject":
                return JsonSerializer.Deserialize(value, typeof(object))!;
            case "secureobject":
                return JsonSerializer.Deserialize(value, typeof(object))!;
            case "secureString":
                return value;
            case "securestring":
                return value;
            case "array":
                return JsonSerializer.Deserialize(value, typeof(object))!;
            default:
                throw new TemplateParsingException($"Couldn't parse {name} as parameter - unsupported type {parameterSchema.Type}.");
        }
    }
}
