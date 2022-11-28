using System.Text.Json;

namespace arm_estimator_tests
{
    internal class MetadataTests
    {
        [Test]
        public async Task WhenAcrUsagePatternIsProvided_ItShouldBeIncludedInEstimation()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/metadata-template.json",
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
            Assert.That(output.TotalCost, Is.EqualTo(0));
            Assert.That(output.Delta, Is.EqualTo(0));
        }
    }
}
