using ACE;

namespace ACE_Tests.Reworked;

[Parallelizable(ParallelScope.Self)]
public class OutputTests
{
    [Test]
    public async Task Output_WhenJsonOutputFilenameContainsJsonExtension_GeneratedOutputFilenameShouldNotAppendOneMoreJsonExtension()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}.json";
        var exitCode = await Program.Main(new[] {
                "templates/reworked/automation-account/automation-account.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--mocked-retail-api-response-path",
                "mocked-responses/retail-api/automation-account/usage-patterns.json"
            });

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText(outputFilename);
        
        Assert.That(outputFile, Is.Not.Null);
    }

    [Test]
    public async Task Output_WhenJsonOutputFilenameDoesNotContainJsonExtension_GeneratedOutputFilenameShouldAppendJsonExtension()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = await Program.Main(new[] {
                "templates/reworked/automation-account/automation-account.bicep",
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
        
        Assert.That(outputFile, Is.Not.Null);
    }
}