using System.Text.Json;
using ACE;

namespace ACE_Tests.Reworked.AutomationAccount;

public class AutomationAccountUsagePatternsTests
{
    [Test]
    [Parallelizable(ParallelScope.Self)]
    public async Task AutomationAccount_UsagePatterns_WhenTemplateDefinesUsagePatterns_TheyShouldAffectCalculation()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = await Program.Main(new[] {
                "templates/reworked/automation-account/usage-patterns.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--mocked-retail-api-response-path",
                "mocked-responses/retail-api/automation-account/usage-patterns.json"
            });

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(5.3200000000000003d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        });

    }
}