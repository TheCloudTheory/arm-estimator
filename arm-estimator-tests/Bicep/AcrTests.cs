using ACE;
using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class AcrTests
    {
        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task ACR_WhenCalculationIsPerformedWithUsage_FreeTierForTasksShouldBeIncluded()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/usagePatterns/acr.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"acrName=acr{DateTime.Now.Ticks}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(5.218d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        }
    }
}
