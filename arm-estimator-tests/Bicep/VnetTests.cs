using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class VnetTests
    {
        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task VNET_WhenCalculationIsPerformedForPeeredVNet_PeeringShouldBeIncludedInCalculation()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/vnet/vnet-peering.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"vnetName1=vnet{DateTime.Now.Ticks}",
                "--inline",
                $"vnetName2=vnet2{DateTime.Now.Ticks}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost, Is.EqualTo(0.02d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task VNET_WhenCalculationIsPerformedForPeeredVNetWithUsagePattern_PeeringShouldBeIncludedInCalculation()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/vnet/vnet-peering-usage.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"vnetName1=vnetu{DateTime.Now.Ticks}",
                "--inline",
                $"vnetName2=vnetu2{DateTime.Now.Ticks}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost, Is.EqualTo(0.02d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
        }
    }
}
