using ACE;
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
        [TestCase("P1", 456.25d)]
        [TestCase("P2", 912.50d)]
        [TestCase("P4", 1825.00d)]
        [TestCase("P6", 3650d)]
        [TestCase("P11", 6868.3875000000007d)]
        [TestCase("P15", 15698.893333333335d)]
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
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
        }

        [Test]
        [TestCase("GP_Gen5_2", 390.54778999999996d)]
        [TestCase("GP_Gen5_4", 780.95872999999995d)]
        [TestCase("GP_Gen5_6", 1171.36967d)]
        [TestCase("GP_Gen5_8", 1561.78061d)]
        [TestCase("GP_Gen5_10", 1952.1915500000002d)]
        [TestCase("GP_Gen5_12", 2342.6024899999998d)]
        [TestCase("GP_Gen5_14", 2733.0134299999995d)]
        [TestCase("GP_Gen5_16", 3123.4243699999997d)]
        [TestCase("GP_Gen5_18", 3513.8353099999995d)]
        [TestCase("GP_Gen5_20", 3904.2462500000001d)]
        [TestCase("GP_Gen5_24", 4685.0681299999997d)]
        [TestCase("GP_Gen5_32", 6246.7118899999996d)]
        [TestCase("GP_Gen5_40", 7808.3556500000004d)]
        [TestCase("GP_Gen5_80", 15616.574450000002d)]
        [Parallelizable(ParallelScope.All)]
        public async Task SQLDatabase_WhenGivenVCoreIsProvided_ItShouldBeCorrectlyEstimated(string sku, double cost)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/sql/sqldatabase-vcore.bicep",
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
            Assert.Multiple(() =>
            {
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(cost));
                Assert.That(output.TotalResourceCount, Is.EqualTo(2));
            });
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task SQLDatabase_WhenVCoreIsSelectedAndConversionRateProvided_ItShouldBeCorrectlyEstimated() 
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/sql/sqldatabase-vcore.bicep",
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
                $"dbSku=GP_Gen5_2",
                "--currency",
                "EUR",
                "--conversion-rate",
                "0.95"
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
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(369.82689999999997d));
                Assert.That(output.TotalResourceCount, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task SQLDatabase_WhenVCoreZoneRedundantIsSelected_ItShouldBeCorrectlyEstimated()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/sql/sqldatabase-zrs.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"serverName=svr{DateTime.Now.Ticks}",
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
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(537.22768586599989d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task SQLDatabase_WhenHybridBenefitIsEnabled_ItShouldBeDeductedFromEstimation()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/usagePatterns/sql.bicep",
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
                $"dbSku=GP_Gen5_2"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(244.59779d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
        }

        [Test]
        [TestCase("Basic", 4.8300000000000001d)]
        [TestCase("S0", 14.718625000000001d)]
        [TestCase("S1", 29.434208333333334d)]
        [TestCase("S2", 73.608333333333334d)]
        [TestCase("S3", 257.67712500000005d)]
        [TestCase("S4", 404.87250000000006d)]
        [TestCase("S6", 699.24500000000012d)]
        [TestCase("S7", 1287.9900000000002d)]
        [TestCase("S9", 2465.4800000000005d)]
        [TestCase("S12", 4526.0874999999996d)]
        [TestCase("P1", 566.75d)]
        [TestCase("P2", 1023.00d)]
        [TestCase("P4", 1935.50d)]
        [TestCase("P6", 3760.50d)]
        [TestCase("P11", 6868.3875000000007d)]
        [TestCase("P15", 15698.893333333335d)]
        [Parallelizable(ParallelScope.All)]
        public async Task SQLDatabase_WhenDatabaseIsDTUAndStorageIsProvided_ItShouldBeAddedToTheEstimation(string sku, double cost)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/sql/sqldatabase-additional-storage.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"serverName=svr{DateTime.Now.Ticks}",
                "--inline",
                $"dbName=db{sku}{DateTime.Now.Ticks}",
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
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task SQLDatabase_WhenUsagePatternForVCoreIsProvided_ItShouldBeUsedInEstimation()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/sql/sqldatabase-vcore-usage.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"serverName=svr{DateTime.Now.Ticks}",
                "--inline",
                $"dbName=dbvu{DateTime.Now.Ticks}"
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
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(408.20143999999999d));
                Assert.That(output.TotalResourceCount, Is.EqualTo(2));
            });
        }

        [Test]
        [TestCase("Basic", 50, 72.6d)]
        [TestCase("Standard", 50, 108.75d)]
        [TestCase("Premium", 125, 675.0d)]
        [Parallelizable(ParallelScope.All)]
        public async Task SQLDatabase_WhenGivenSkuIsProvidedForElasticPool_ItShouldBeCorrectlyEstimated(string sku, int capacity, double cost)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/sql/sqldatabase-elasticpool.bicep",
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
                $"elasticPoolSku={sku}",
                "--inline",
                $"elasticPoolCapacity={capacity}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(3));
        }
    }
}
