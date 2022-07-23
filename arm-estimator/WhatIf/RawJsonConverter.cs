using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// https://stackoverflow.com/a/70477321/1874991
/// </summary>
public class RawJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        return doc.RootElement.GetRawText();
    }

    protected virtual bool SkipInputValidation => false;

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) =>
        writer.WriteRawValue(value, skipInputValidation : SkipInputValidation);
}