using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class LogAnalyticsTests
    {
        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task LogAnalytics_WhenDailyQuotaIsProvided_EstimationShouldBeBasedOnIt()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/infer-log-analytics-cost.bicep",
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
            Assert.That(output.TotalCost, Is.EqualTo(457.64999999999998d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
        }
    }
}
