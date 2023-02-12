using ACE;
using System.Text.Json;

namespace arm_estimator_tests
{
    internal class OutputTests
    {
        [Test]
        [Parallelizable(ParallelScope.Self)]
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
            Assert.That(output.Resources.Count(), Is.EqualTo(1));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
            Assert.That(output.EstimatedResourceCount, Is.EqualTo(1));
            Assert.That(output.SkippedResourceCount, Is.EqualTo(1));
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task WhenTableOutputIsUsed_ItShouldWorkWithoutError()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/sql/sqldatabase.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"serverName=svrt{DateTime.Now.Ticks}",
                 "--inline",
                $"dbName=sqldbt{DateTime.Now.Ticks}",
                 "--inline",
                $"dbSku=Basic",
                "--outputFormat",
                "Table"
            });

            Assert.That(exitCode, Is.EqualTo(0));
        }
    }
}
