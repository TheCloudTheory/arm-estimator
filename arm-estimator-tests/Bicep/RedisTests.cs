using ACE;
using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class RedisTests
    {
        [Test]
        [TestCase("Enterprise_E10", 975.27999999999997d, 2)]
        [TestCase("Enterprise_E10", 1950.5599999999999d, 4)]
        [TestCase("Enterprise_E20", 1945.4500000000003d, 2)]
        [TestCase("Enterprise_E20", 3890.9000000000005d, 4)]
        [TestCase("Enterprise_E50", 3822.2799999999997d, 2)]
        [TestCase("Enterprise_E50", 7644.5599999999995d, 4)]
        [TestCase("Enterprise_E100", 9293.630000000001d, 2)]
        [TestCase("Enterprise_E100", 18587.260000000002d, 4)]
        [TestCase("EnterpriseFlash_F300", 7920.5730000000003d, 3)]
        [TestCase("EnterpriseFlash_F300", 23761.719000000001d, 9)]
        [TestCase("EnterpriseFlash_F700", 15840.927000000001d, 3)]
        [TestCase("EnterpriseFlash_F700", 47522.781000000003d, 9)]
        [TestCase("EnterpriseFlash_F1500", 31682.0d, 3)]
        [TestCase("EnterpriseFlash_F1500", 95046.0d, 9)]
        [Parallelizable(ParallelScope.All)]
        public async Task Redis_WhenGivenSkuIsProvided_ItShouldBeCorrectlyEstimated(string sku, double cost, int capacity)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/redis/redis-enterprise.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"name=redis{DateTime.Now.Ticks}",
                "--inline",
                $"capacity={capacity}",
                "--inline",
                $"skuName={sku}"
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
