using System.Text.Json;
using System.Text.Json.Serialization;

internal class TemplateSchema
{
    [JsonPropertyName("parameters")]
    public IDictionary<string, TemplateParameter>? Parameters { get; set; }

    [JsonPropertyName("metadata")]
    public MetadataSchema? Metadata { get; set; }

    [JsonPropertyName("resources")]
    [JsonConverter(typeof(Language20ResourceSchemaConverter))]
    public SpecialCaseResourceSchema[]? SpecialCaseResources { get; set; }
}

internal class MetadataSchema
{
    [JsonPropertyName("aceUsagePatterns")]
    public IDictionary<string, string>? UsagePatterns { get; set; }
}

internal class TemplateParameter
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    public object? Value { get; set; }
}

internal class SpecialCaseResourceSchema
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("properties")]
    [JsonConverter(typeof(SpecialCaseResourcePropertiesSchemaConverter))]
    public SpecialCaseResourcePropertiesSchema? Properties { get; set; } = null!;
}

internal class SpecialCaseResourcePropertiesSchema
{
    [JsonPropertyName("virtualNetworkPeerings")]
    public VirtualNetworkPeeringSchema[]? VirtualNetworkPeerings { get; set; }
}

internal class VirtualNetworkPeeringSchema
{
    [JsonPropertyName("id")]
    public string? Id { get; set; } = null!;
}

/// <summary>
/// Custom converter for <see cref="SpecialCaseResourcePropertiesSchema"/>. This is needed because the properties
/// can be either an object or a string representing dynamically generated object, and the default converter doesn't handle that.
/// </summary>
internal class SpecialCaseResourcePropertiesSchemaConverter : JsonConverter<SpecialCaseResourcePropertiesSchema>
{
    public override SpecialCaseResourcePropertiesSchema? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType == JsonTokenType.Null) return null;
        if(reader.TokenType == JsonTokenType.StartObject)
        {
            var value = JsonSerializer.Deserialize<SpecialCaseResourcePropertiesSchema>(ref reader, options);
            return value;
        }

        // If the value is not an object, it's most likely a string. As for now, we're ignoring such case.
        return null;
    }

    public override void Write(Utf8JsonWriter writer, SpecialCaseResourcePropertiesSchema value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

internal class Language20ResourceSchemaConverter : JsonConverter<SpecialCaseResourceSchema[]?>
{
    public override SpecialCaseResourceSchema[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType == JsonTokenType.Null) return null;
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var value = JsonSerializer.Deserialize<SpecialCaseResourceSchema[]>(ref reader, options);
            return value;
        }
        
        if(reader.TokenType == JsonTokenType.StartObject)
        {
            var value = JsonSerializer.Deserialize<IDictionary<string, SpecialCaseResourceSchema>>(ref reader, options);
            return value!.Select(r => r.Value).ToArray();
        }
        
        return null;
    }

    public override void Write(Utf8JsonWriter writer, SpecialCaseResourceSchema[]? value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}