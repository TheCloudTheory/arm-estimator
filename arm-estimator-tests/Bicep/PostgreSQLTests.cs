using ACE;
using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class PostgreSQLTests
    {
        [Test]
        [TestCase("Disabled", 70.603000000000009d)]
        [TestCase("Enabled", 82.503d)]
        [Parallelizable(ParallelScope.All)]
        public async Task PostgreSQL_WhenCalculationIsPerformedWithBackupUsagePattern_ItShouldBeCalculatedCorrectly(string geoRedundantBackup, double cost)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/usagePatterns/postgresql.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"name=postgresql{DateTime.Now.Ticks}",
                "--inline",
                $"geoRedundantBackup={geoRedundantBackup}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        }
    }
}
