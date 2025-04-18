using System.Text.Json;
using ACE;

namespace ACE_Tests.Reworked.AutomationAccount;

public class AutomationAccountTests
{
    [Test]
    [Parallelizable(ParallelScope.Self)]
    public void AutomationAccount_Generic_WhenAutomationAccountIsDefined_CalculationShouldIncludeFreeTier()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = Program.Main([
                "templates/reworked/automation-account/automation-account.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generate-json-output",
                "--json-output-filename",
                outputFilename,
                "--mocked-retail-api-response-path",
                "mocked-responses/retail-api/automation-account/usage-patterns.json"
            ]);

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        });
    }
}