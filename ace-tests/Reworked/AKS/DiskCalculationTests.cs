using System.Text.Json;
using ACE;
using arm_estimator_tests;

namespace ACE_Tests.Reworked.AKS;

public class DiskCalculationTests
{
    [Test]
    public async Task AKS_DiskCalculation_WhenTemplateDefinesUltraSSDDisk_ItShouldBeCalculatedCorrectly()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = await Program.Main(new[] {
                "templates/reworked/aks/ultrassd.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--mocked-retail-api-response-path",
                "mocked-responses/retail-api/aks/ultrassd.json"
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
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(162.22352000000001d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        });

    }
}
