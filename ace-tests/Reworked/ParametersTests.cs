using System.Text.Json;
using ACE;

namespace ACE_Tests.Reworked;

[Parallelizable(ParallelScope.Self)]
public class ParametersTests
{
    [Test]
    public async Task Parameters_Bicepparam_WhenOnlyBicepparamParametersAreProvided_ItShouldProperlyUseThemAndEstimateTemplate()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}.json";
        var exitCode = await Program.Main(new[] {
                "templates/reworked/automation-account/automation-parameters.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--parameters",
                "templates/reworked/automation-account/automation-parameters.bicepparam",
                "--mocked-retail-api-response-path",
                "mocked-responses/retail-api/automation-account/usage-patterns.json"
            });

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        });
        
    }

    [Test]
    public async Task Parameters_Bicepparam_WhenBothBicepparamAndInlineParametersAreProvided_ItShouldProperlyUseThemAndEstimateTemplate()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}.json";
        var exitCode = await Program.Main(new[] {
                "templates/reworked/automation-account/automation-parameters.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--parameters",
                "templates/reworked/automation-account/automation-parameters.bicepparam",
                "--inline",
                "parSuffix=bicepparam",
                "--mocked-retail-api-response-path",
                "mocked-responses/retail-api/automation-account/usage-patterns.json"
            });

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        });
    }
}