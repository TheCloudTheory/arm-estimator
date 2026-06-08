internal class StreamAnalyticsJobTransformation
{
    public string? name { get; set; }
    public StreamAnalyticsJobTransformationProperties? properties { get; set; }
}

internal class StreamAnalyticsJobTransformationProperties
{
    public int? streamingUnits { get; set; }
}