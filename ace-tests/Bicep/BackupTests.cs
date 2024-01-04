using ACE;
using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    [Ignore("Under rework")]
    internal class BackupTests
    {
        [Test]
        [TestCase("GeoRedundant", 132.6848d)]
        [TestCase("LocallyRedundant", 132.66239999999999d)]
        [TestCase("ZoneRedundant", 132.66800000000001d)]
        [TestCase("ReadAccessGeoZoneRedundant", 132.6969d)]
        [Parallelizable(ParallelScope.All)]
        public async Task Backup_WhenRedundancyModeIsProvided_ItShouldBeCorrectlyEstimated(string mode, double cost)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/backup/backup-redundancy.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"redundancyMode={mode}",
                "--inline",
                $"resourceNamePrefix=ace{DateTime.Now.Ticks}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(13));
            Assert.That(output.EstimatedResourceCount, Is.EqualTo(13));
        }

        [Test]
        [TestCase("GeoRedundant", 134.88d)]
        [TestCase("LocallyRedundant", 133.75999999999999d)]
        [TestCase("ZoneRedundant", 134.04000000000002d)]
        [TestCase("ReadAccessGeoZoneRedundant", 135.48500000000001d)]
        [Parallelizable(ParallelScope.All)]
        public async Task Backup_WhenRedundancyModeIsProvidedWithUsagePatterns_ItShouldBeCorrectlyEstimated(string mode, double cost)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/usagePatterns/backup.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"redundancyMode={mode}",
                "--inline",
                $"resourceNamePrefix=ace{mode[..2]}{DateTime.Now.Ticks}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(13));
            Assert.That(output.EstimatedResourceCount, Is.EqualTo(13));
        }
    }
}
