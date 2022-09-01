using System.Text.Json;

namespace arm_estimator_tests
{
    public class Generic_ARM_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("templates/acr.json", 2.8001, 2.8001)]
        [TestCase("templates/alert.json", 2.8001, 2.8001)]
        [TestCase("templates/analysisservices.json", 14389.759999999998, 14389.759999999998)]
        public async Task ResourceEstimation_ShouldBeCalculatedCorrectly(string templatePath, double totalValue, double deltaValue)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                templatePath, 
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
            Assert.That(output.TotalCost, Is.EqualTo(totalValue));
            Assert.That(output.Delta, Is.EqualTo(deltaValue));
        }
    }
}