using System.Text.Json;

namespace arm_estimator_tests
{
    public class AzureContainerRegistry_ARM_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task AzureContainerRegistryEstimation_ShouldBeCalculatedCorrectly()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] { 
                "templates/acr.json", 
                "f81e70a7-e819-49b2-a980-8e9c433743dd", 
                "arm-estimator-rg", 
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
            Assert.That(output.TotalCost, Is.EqualTo(2.8001));
            Assert.That(output.Delta, Is.EqualTo(2.8001));
        }
    }
}