using System.Text.Json;
using System.Text.Json.Serialization;
using ACE.Output;
using ACE.WhatIf;

namespace ACE;

internal class Configuration
{
    public DeploymentMode Mode { get; set; }
    public int? Threshold { get; set; }
    public FileInfo? Parameters { get; set; }
    public CurrencyCode Currency { get; set; }
    public bool GenerateJsonOutput { get; set; }
    public bool Silent { get; set; }
    public bool Stdout { get; set; }
    public bool DisableDetailedMetrics { get; set; }
    public string? JsonOutputFilename { get; set; }
    public bool GenerateHtmlOutput { get; set; }
    public bool DryRun { get; set; }
    public string? HtmlOutputFilename { get; set; }
    public OutputFormat OutputFormat { get; set; }
    public bool DisableCache { get; set; }
    public string? TfExecutable { get; set; }
    public double ConversionRate { get; set; }
    public CacheHandler CacheHandler { get; set; }
    public string? CacheStorageAccountName { get; set; }
    public string? WebhookUrl { get; set; }
    public string? LogFile { get; set; }
    public bool DisableVersionCheck { get; set; }
    
    [JsonConverter(typeof(StringToFileInfoConverter))]
    public FileInfo[]? MockedRetailAPIResponsePaths { get; set; }
    public bool Debug { get; set; }
    public string? UserGeneratedWhatIf { get; set; }
    public bool GenerateMarkdownOutput { get; set; }
    public string? MarkdownOutputFilename { get; set; }
    public bool ForceUsingBicepCli { get; set; }
}

internal class StringToFileInfoConverter : JsonConverter<FileInfo[]>
{
    public override FileInfo[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            reader.Read();
            var fileName = reader.GetString();
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("File name is empty and can't be deserialized to FileInfo.");    
            }
            
            var fi = new FileInfo(fileName);
            
            // We're inside reading an array so we need to read EndArray token as well
            reader.Read();
            
            
            return [fi];
        }
        
        return [];
    }

    public override void Write(Utf8JsonWriter writer, FileInfo[] value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}