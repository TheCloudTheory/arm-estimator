using System.Text.Json;

namespace arm_estimator_tests
{
    internal class NestedResourcesTests
    {
        [Test]
        public async Task WhenNestedResourcesArePresent_TheyShouldBeDetected()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/nested.bicep",
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
            Assert.That(output.Resources.Count(), Is.EqualTo(3));
        }
    }
}
