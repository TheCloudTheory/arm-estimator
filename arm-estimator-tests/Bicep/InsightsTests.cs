using ACE;
using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class InsightsTests
    {
        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task Insights_WhenDiagnosticSettingsAreDefined_AdditionalCostShouldBeAdded()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/insights/diagnostic-settings.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"dsName=ds{DateTime.Now.Ticks}",
                "--inline",
                $"dsName2=ds2{DateTime.Now.Ticks}",
                "--inline",
                $"dsName3=ds3{DateTime.Now.Ticks}",
                "--inline",
                $"workspaceId={DateTime.Now.Ticks}",
                "--inline",
                $"storageAccountId={DateTime.Now.Ticks}",
                "--inline",
                $"eventHubName={DateTime.Now.Ticks}",
                "--inline",
                $"serverName=srv{DateTime.Now.Ticks}",
                "--inline",
                $"dbName=db{DateTime.Now.Ticks}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(19.060359999999999d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(5));
        }
    }
}
