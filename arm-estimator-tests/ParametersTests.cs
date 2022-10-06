using System.Text.Json;

namespace arm_estimator_tests
{
    internal class ParametersTests
    {
        [Test]
        public async Task WhenNestedResourcesArePresent_TheyShouldBeDetected()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/securestring.bicep",
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
            Assert.That(output.Resources.Count(), Is.EqualTo(3));
        }
    }
}
