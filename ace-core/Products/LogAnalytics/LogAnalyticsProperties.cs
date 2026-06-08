using System.Text.Json.Serialization;

internal class LogAnalyticsProperties
{
    [JsonPropertyName("workspaceCapping")]
    public WorkspaceCapping? WorkspaceCapping { get; set; }
}

internal class WorkspaceCapping
{
    [JsonPropertyName("dailyQuotaGb")]
    public double DailyQuotaGB { get; set; }
}