﻿using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class BackupTests
    {
        [Test]
        [TestCase("GeoRedundant", 113.48479999999999d)]
        [TestCase("LocallyRedundant", 113.4624d)]
        [TestCase("ZoneRedundant", 113.468d)]
        [TestCase("ReadAccessGeoZoneRedundant", 113.4969d)]
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
            Assert.That(output.TotalCost, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(13));
            Assert.That(output.EstimatedResourceCount, Is.EqualTo(13));
        }

        [Test]
        [TestCase("GeoRedundant", 115.67999999999999d)]
        [TestCase("LocallyRedundant", 114.56d)]
        [TestCase("ZoneRedundant", 114.84d)]
        [TestCase("ReadAccessGeoZoneRedundant", 116.285d)]
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
            Assert.That(output.TotalCost, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(13));
            Assert.That(output.EstimatedResourceCount, Is.EqualTo(13));
        }
    }
}
