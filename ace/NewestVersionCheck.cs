using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ACE;

internal class NewestVersionCheck
{
    private const string GitHubReleasesUrl = "https://api.github.com/repos/thecloudtheory/arm-estimator/releases";
    private readonly ILogger<Program> logger;

    public NewestVersionCheck(ILogger<Program> logger)
    {
        this.logger = logger;
    }

    public async Task DisplayCheckInfoAsync(string version, bool isNewVersionCheckDisabled)
    {
        if (isNewVersionCheckDisabled == true)
        {
            this.logger.LogInformation("Checking for new version of ACE is disabled.");
            this.logger.LogInformation("");
            this.logger.LogInformation("------------------------------");
            this.logger.LogInformation("");
        }
        else
        {
            var latestVersion = await GetLatestVersionAsync();
            if (latestVersion != null)
            {
                var currentVersion = version;
                if (currentVersion != null && latestVersion != currentVersion)
                {
                    this.logger.LogInformation("New version of ACE is available. Current version: {currentVersion}, latest version: {latestVersion}", currentVersion, latestVersion);
                    this.logger.LogInformation("Please update ACE to the latest version.");
                    this.logger.LogInformation("");
                    this.logger.LogInformation("------------------------------");
                    this.logger.LogInformation("");
                }
            }
        }
    }

    private async Task<string> GetLatestVersionAsync()
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "arm-estimator");
            var response = await client.GetAsync(GitHubReleasesUrl);
            response.EnsureSuccessStatusCode();

            var responseContent = response.Content;
            if (responseContent != null)
            {
                var responseString = responseContent.ReadAsStringAsync().Result;
                var releases = JsonSerializer.Deserialize<List<GitHubRelease>>(responseString);
                if (releases != null)
                {
                    var latestRelease = releases.FirstOrDefault(_ => _.draft == false);
                    if (latestRelease != null)
                    {
                        return latestRelease.tag_name;
                    }
                    else
                    {
                        throw new Exception("Failed to get latest version of ACE.");
                    }
                }
                else
                {
                    throw new Exception("Failed to get latest version of ACE.");
                }
            }
            else
            {
                throw new Exception("Failed to get latest version of ACE.");
            }
        }

    }
}

internal class GitHubRelease
{
    public string name { get; set; } = null!;
    public bool draft { get; set; }
    public string tag_name { get; set; } = null!;
}