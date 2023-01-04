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
                "templates/bicep/metadata-template.bicep",
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
            Assert.That(output.TotalCost, Is.EqualTo(7.4990000000000006d));
            Assert.That(output.Delta, Is.EqualTo(7.4990000000000006d));
        }
    }
}
