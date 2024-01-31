using System.Text.Json;
using ACE;

namespace ACE_Tests.Reworked.SiteRecovery;

[Parallelizable(ParallelScope.Self)]
public class SiteRecoveryInferredMetricsTests
{
    [Test]
    public async Task SiteRecovery_Inferred_WhenThereIsAzureVMReplicated_ItShouldBeInferredForCalculation()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = await Program.Main(new[] {
                "templates/reworked/site-recovery/replicated-vm.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename
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
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(246.01479999999998d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(7));
        });
    }
}