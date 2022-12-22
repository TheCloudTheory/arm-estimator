using System.Text.Json;

namespace arm_estimator_tests
{
    internal class ParametersTests
    {
        [Test]
        public async Task WhenSecureStringIsProvidedInline_ItShouldBeUsedInEstimate()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/securestring.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                "adminPassword=verysecretpassword123"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.Resources.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task WhenMultipleParamsAreUsedInline_TheyShouldBeUsedInEstimate()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/inlineParams.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                "adminPassword=verysecretpassword123",
                "--inline",
                "adminLogin=adminace",
                "--inline",
                "minCapacity=3",
                "--inline",
                "singleLineObject={\"name\": \"test name\", \"id\": \"123-abc\", \"isCurrent\": true, \"tier\": 1}",
                "--inline",
                "exampleArray=[\"1\", \"2\", \"3\"]"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.Resources.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task WhenBothParametersFileAndInlineParametersAreUsed_TheyShouldBeUsedInEstimate()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/106-file-and-inline-params.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--parameters",
                "templates/106-file-and-inline-params.parameters.json",
                "--inline",
                "adminPassword=verysecretpassword123"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.Resources.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task WhenBothParametersFileAndInlineParametersAreUsed_TheyShouldMergeCleanly()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/106-file-and-inline-params.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--parameters",
                "templates/106-file-and-inline-params.parameters.json",
                "--inline",
                "dbName=anOverridenDbName",
                "--inline",
                "adminPassword=verysecretpassword123"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.Resources.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task WhenDryRunIsEnabled_EstimationShouldNotBePerformed()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/106-file-and-inline-params.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--parameters",
                "templates/106-file-and-inline-params.parameters.json",
                "--inline",
                "dbName=dryrundb",
                "--inline",
                "adminPassword=verysecretpassword123",
                "--dry-run"
            });

            Assert.That(exitCode, Is.EqualTo(0));
        }
    }
}
