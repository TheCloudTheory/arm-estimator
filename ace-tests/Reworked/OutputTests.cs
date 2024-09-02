namespace ACE_Tests.Reworked;

[Parallelizable(ParallelScope.Self)]
public class OutputTests
{
    [Test]
    public void Output_WhenJsonOutputFilenameContainsJsonExtension_GeneratedOutputFilenameShouldNotAppendOneMoreJsonExtension()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}.json";
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

        var outputFile = File.ReadAllText(outputFilename);
        
        Assert.That(outputFile, Is.Not.Null);
    }

    [Test]
    public void Output_WhenJsonOutputFilenameDoesNotContainJsonExtension_GeneratedOutputFilenameShouldAppendJsonExtension()
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
        
        Assert.That(outputFile, Is.Not.Null);
    }

    [Test]
    public void Output_WhenMarkdownOutputIsRequested_ItShouldGenerateWithoutError()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = Program.Main([
                "templates/reworked/automation-account/automation-account.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generate-markdown-output",
                "--markdown-output-filename",
                outputFilename,
                "--mocked-retail-api-response-path",
                "mocked-responses/retail-api/automation-account/usage-patterns.json"
            ]);

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.md");
        
        Assert.That(outputFile, Is.Not.Null);
    }
}