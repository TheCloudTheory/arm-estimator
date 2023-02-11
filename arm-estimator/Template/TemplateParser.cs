using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.Json;

internal class TemplateParser
{
    public TemplateSchema? Template { get; private set; }
    private readonly string parameters;
    private readonly IEnumerable<string>? inlineParameters;
    private readonly ILogger logger;

    public TemplateParser(string template, string parameters, IEnumerable<string>? inlineParameters, ILogger logger)
    {
        Template = JsonSerializer.Deserialize<TemplateSchema>(template);

        this.parameters = parameters;
        this.inlineParameters = inlineParameters;
        this.logger = logger;
    }

    public void ParseInlineParameters(out string parameters)
    {
        var parsedParameters = JsonSerializer.Deserialize<ParametersSchema>(this.parameters);
        if (parsedParameters != null)
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

                parsedParameters.Parameters ??= new Dictionary<string, Parameter>();
                if (parsedParameters.Parameters.ContainsKey(parameterName))
                {
                    parsedParameters.Parameters[parameterName].Value = value;
                }
                else
                {
                    parsedParameters.Parameters.Add(parameterName, new Parameter() { Value = value });
                }
            }

            parameters = JsonSerializer.Serialize(parsedParameters, new JsonSerializerOptions()
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
