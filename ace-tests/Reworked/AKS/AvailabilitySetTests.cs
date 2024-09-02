using System.Text.Json;
using ACE;

namespace ACE_Tests.Reworked.AKS;

public class AvailabilitySetTests
{
    [Test]
    [Parallelizable(ParallelScope.Self)]
    public void AKS_AvailabilitySet_WhenTemplateDefinesAvailabilitySet_ItShouldBeCalculatedCorrectly()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = Program.Main([
                "templates/reworked/aks/availability-set.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generate-json-output",
                "--json-output-filename",
                outputFilename,
                "--mocked-retail-api-response-path",
                "mocked-responses/retail-api/aks/availability-set.json"
            ]);

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(176.44d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        });

    }
}