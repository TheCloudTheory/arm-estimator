using ACE;
using System.Text.Json;

namespace arm_estimator_tests
{
    public class Generic_ARM_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("templates/acr.json", 75.294000000000011d, 75.294000000000011d)]
        [TestCase("templates/alert.json", 13.599999999999998d, 13.599999999999998d)]
        [TestCase("templates/analysisservices.json", 14389.759999999998, 14389.759999999998)]
        [TestCase("templates/mariadb.json", 19526.757600000001d, 19526.757600000001d)]
        [TestCase("templates/automation.json", 5.5399999999999991d, 5.5399999999999991d)]
        [TestCase("templates/redis.json", 12339.190000000001, 12339.190000000001)]
        [TestCase("templates/azure_firewall.json", 2197.348, 2197.348)]
        [TestCase("templates/frontdoor.json", 0.0, 0.0)] // Not supported yet
        [TestCase("templates/azuresqlmi.json", 0.0, 0.0)] // Not supported yet
        [TestCase("templates/mysqldb.json", 0.0, 0.0)] // Not supported yet
        [TestCase("templates/postgresql.json", 10191.206299999998d, 10191.206299999998d)]
        [TestCase("templates/servicebus.json", 688.50693000000001d, 688.50693000000001d)]
        [TestCase("templates/datafactory.json", 189.13086000000001d, 189.13086000000001d)]
        [TestCase("templates/appgw.json", 1598.7849000000001d, 1598.7849000000001d)]
        [Parallelizable(ParallelScope.All)]
        public async Task ResourceEstimation_ShouldBeCalculatedCorrectly(string templatePath, double totalValue, double deltaValue)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                templatePath,
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
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(totalValue));
            Assert.That(output.Delta.OriginalValue, Is.EqualTo(deltaValue));
        }

        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task AksResourceEstimation_ShouldBeCalculatedCorrectly()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/107-aks-support.json",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--parameters",
                "templates/107-aks-support.parameters.json",
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
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(1136.1199999999997d));
        }
    }
}