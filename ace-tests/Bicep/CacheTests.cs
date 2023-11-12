using ACE;
using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class CacheTests
    {
        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task Cache_WhenCalculationIsPerformedTwice_ItShouldUseFirstWhatIf()
        {
            var acrName = $"acr{DateTime.Now.Ticks}";

            await RunAcrEstimation(acrName);
            await RunAcrEstimation(acrName);
        }

        private static async Task RunAcrEstimation(string acrName)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/cache/cache.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"acrName={acrName}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(5.0979999999999999d));
                Assert.That(output.TotalResourceCount, Is.EqualTo(1));
            });
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task Cache_WhenUsingAzureStorageCache_ItShouldUseFirstWhatIf()
        {
            var acrName = $"acr{DateTime.Now.Ticks}";

            await RunAcrEstimationWithAzureStorageCache(acrName);
            await RunAcrEstimationWithAzureStorageCache(acrName);
        }

        private static async Task RunAcrEstimationWithAzureStorageCache(string acrName)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/cache/cache.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"acrName={acrName}",
                "--cache-handler",
                "AzureStorage",
                "--cache-storage-account-name",
                "acecachesa"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(5.0979999999999999d));
                Assert.That(output.TotalResourceCount, Is.EqualTo(1));
            });
        }
    }
}
