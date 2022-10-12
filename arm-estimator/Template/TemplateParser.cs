using arm_estimator.Template;
using Microsoft.Extensions.Logging;
using System.Text.Json;

internal class TemplateParser
{
    private readonly TemplateSchema? template;
    private readonly string parameters;
    private readonly IEnumerable<string> inlineParameters;
    private readonly ILogger logger;

    public TemplateParser(string template, string parameters, IEnumerable<string> inlineParameters, ILogger logger)
    {
        this.template = JsonSerializer.Deserialize<TemplateSchema>(template); ;
        this.parameters = parameters;
        this.inlineParameters = inlineParameters;
        this.logger = logger;
    }

    public void ParseInlineParameters(out string parameters)
    {
        var parsedParameters = JsonSerializer.Deserialize<ParametersSchema>(this.parameters);
        if (parsedParameters != null)
        {
            if(this.template == null)
            {
                this.logger.LogError("Couldn't parse template.");
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

                if (parsedParameters.Parameters == null)
                {
                    parsedParameters.Parameters = new Dictionary<string, Parameter>();
                }

                parsedParameters.Parameters.Add(parameterName, new Parameter() { Value = value });
            }

            parameters = JsonSerializer.Serialize(parsedParameters, new JsonSerializerOptions()
            {
                WriteIndented = false
            });
        } 
        else
        {
            parameters = "{}";
        }
    }

    private object ParseParamaterByTheirType(string name, string value)
    {
        if(this.template!.Parameters == null)
        {
            throw new TemplateParsingException("Parameters cannot be null when parsed.");
        }

        if(this.template!.Parameters.TryGetValue(name, out var parameterSchema) == false)
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
                return value;
            case "bool":
                return bool.Parse(value);
            case "int":
                return int.Parse(value);
            case "object":
                return JsonSerializer.Deserialize(value, typeof(object))!;
            case "secureObject":
                return JsonSerializer.Deserialize(value, typeof(object))!;
            case "secureString":
                return value;
            case "array":
                return JsonSerializer.Deserialize(value, typeof(object))!;
            default:
                throw new TemplateParsingException($"Couldn't parse {name} as parameter - unsupported type.");
        }
    }
}
