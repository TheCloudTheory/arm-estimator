using System.Text.Json;
using arm_estimator_tests;

namespace ACE.Tests;

public class ConfigurationTests
{
    [Test]
    [Parallelizable(ParallelScope.Self)]
    public async Task WhenConfigurationIsPassedViaConfigurationFile_ItShouldBeParsedCorrectly()
    {
        var exitCode = await Program.Main(new[] {
                "templates/bicep/availability-set.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--configuration-file",
                "templates/configuration/configuration.json"
            });

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"output_configuration_test.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(211.69999999999999d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(4));
        });
    }

    [Test]
    [Parallelizable(ParallelScope.Self)]
    public async Task WhenConfigurationIsPassedViaConfigurationFileAndInline_ItShouldReturnError()
    {
        var exitCode = await Program.Main(new[] {
                "templates/bicep/availability-set.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--configuration-file",
                "templates/configuration/configuration.json",
                "--generateJsonOutput"
            });

        Assert.That(exitCode, Is.EqualTo(1));
    }
}