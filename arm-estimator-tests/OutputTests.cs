using System.Text.Json;

namespace arm_estimator_tests
{
    internal class OutputTests
    {
        [Test]
        public async Task WhenTemplateContainsBothSupportedAndNotSupportedResources_ItShouldGenerateOutputFileWithProperSummary()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/unsupported-and-supported.json",
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
            Assert.That(output.Resources.Count(), Is.EqualTo(2));
            Assert.That(output.TotalResourceCount, Is.EqualTo(3));
            Assert.That(output.EstimatedResourceCount, Is.EqualTo(2));
            Assert.That(output.SkippedResourceCount, Is.EqualTo(1));
        }
    }
}
