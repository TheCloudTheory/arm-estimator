using System.Text.Json;
using ACE;

namespace ACE_Tests.Reworked.VirtualNetwork;

[Parallelizable(ParallelScope.Self)]
public class VirtualNetworkPeeringTests
{
    [Test]
    public void VirtualNetwork_Peering_WhenThereIsAzureVirtualNetworkPeeringBetweenZones1And2_ItShouldBeInferredForCalculation()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = Program.Main([
                    "templates/reworked/virtual-network/peering-1-2.bicep",
                    "cf70b558-b930-45e4-9048-ebcefb926adf",
                    "arm-estimator-tests-rg",
                    "--generate-json-output",
                    "--json-output-filename",
                    outputFilename
                ]);

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0.25d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(4));
        });
    }

    [Test]
    public void VirtualNetwork_Peering_WhenThereIsAzureVirtualNetworkPeeringBetweenZones1And3_ItShouldBeInferredForCalculation()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = Program.Main([
                    "templates/reworked/virtual-network/peering-1-3.bicep",
                    "cf70b558-b930-45e4-9048-ebcefb926adf",
                    "arm-estimator-tests-rg",
                    "--generate-json-output",
                    "--json-output-filename",
                    outputFilename
                ]);

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0.39d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(4));
        });
    }

    [Test]
    public void VirtualNetwork_Peering_WhenThereIsAzureVirtualNetworkPeeringBetweenZones2And3_ItShouldBeInferredForCalculation()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = Program.Main([
                    "templates/reworked/virtual-network/peering-2-3.bicep",
                    "cf70b558-b930-45e4-9048-ebcefb926adf",
                    "arm-estimator-tests-rg",
                    "--generate-json-output",
                    "--json-output-filename",
                    outputFilename
                ]);

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0.5d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(4));
        });
    }
}
