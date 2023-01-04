using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class SqlTests
    {
        [Test]
        [TestCase("Basic", 4.8300000000000001d)]
        [TestCase("S0", 14.718625000000001d)]
        [TestCase("S1", 29.434208333333334d)]
        [TestCase("S2", 73.608333333333334d)]
        [TestCase("S3", 147.17712500000002d)]
        [TestCase("S4", 294.37250000000006d)]
        [TestCase("S6", 588.74500000000012d)]
        [TestCase("S7", 1177.4900000000002d)]
        [TestCase("S9", 2354.9800000000005d)]
        [TestCase("S12", 4415.5874999999996d)]
        [Parallelizable(ParallelScope.All)]
        public async Task SQLDatabase_WhenGivenSkuIsProvided_ItShouldBeCorrectlyEstimated(string sku, double cost)
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
                $"serverName=svr{DateTime.Now.Ticks}",
                "--inline",
                $"dbName=db{DateTime.Now.Ticks}",
                "--inline",
                $"dbSku={sku}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
        }
    }
}
