using System.Text.Json;
using ACE;

namespace ACE_Tests.Reworked.VirtualNetwork
{
    [Parallelizable(ParallelScope.Self)]
    public class VirtualNetworkPeeringTests
    {
        [Test]
        public async Task VirtualNetwork_Peering_WhenThereIsAzureVirtualNetworkPeeringBetweenZones1And2_ItShouldBeInferredForCalculation()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                    "templates/reworked/virtual-network/peering-1-2.bicep",
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
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0.0d));
                Assert.That(output.TotalResourceCount, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task VirtualNetwork_Peering_WhenThereIsAzureVirtualNetworkPeeringBetweenZones1And3_ItShouldBeInferredForCalculation()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                    "templates/reworked/virtual-network/virtual-network-peering.bicep",
                    "cf70b558-b930-45e4-9048-ebcefb926adf",
                    "arm-estimator-tests-rg",
                    "--generateJsonOutput",
                    "--jsonOutputFilename",
                    outputFilename,
                    "--mocked-retail-api-response-path",
                    "mocked-responses/retail-api/vnet/peering-1.json",
                    "--mocked-retail-api-response-path",
                    "mocked-responses/retail-api/vnet/peering-2.json",
                    "--mocked-retail-api-response-path",
                    "mocked-responses/retail-api/vnet/peering-3.json"
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
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0.0d));
                Assert.That(output.TotalResourceCount, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task VirtualNetwork_Peering_WhenThereIsAzureVirtualNetworkPeeringBetweenZones2And3_ItShouldBeInferredForCalculation()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                    "templates/reworked/virtual-network/virtual-network-peering.bicep",
                    "cf70b558-b930-45e4-9048-ebcefb926adf",
                    "arm-estimator-tests-rg",
                    "--generateJsonOutput",
                    "--jsonOutputFilename",
                    outputFilename,
                    "--mocked-retail-api-response-path",
                    "mocked-responses/retail-api/vnet/peering-1.json",
                    "--mocked-retail-api-response-path",
                    "mocked-responses/retail-api/vnet/peering-2.json",
                    "--mocked-retail-api-response-path",
                    "mocked-responses/retail-api/vnet/peering-3.json"
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
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0.0d));
                Assert.That(output.TotalResourceCount, Is.EqualTo(1));
            });
        }
    }
}