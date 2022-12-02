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
        [TestCase("templates/acr.json", 75.294300000000007d, 75.294300000000007d)]
        [TestCase("templates/alert.json", 13.599999999999998d, 13.599999999999998d)]
        [TestCase("templates/analysisservices.json", 14389.759999999998, 14389.759999999998)]
        [TestCase("templates/mariadb.json", 19525.8056d, 19525.8056d)]
        [TestCase("templates/automation.json", 8.5300000000000011d, 8.5300000000000011d)]
        [TestCase("templates/redis.json", 12339.190000000001, 12339.190000000001)]
        [TestCase("templates/azure_firewall.json", 2197.348, 2197.348)]
        [TestCase("templates/frontdoor.json", 0.0, 0.0)] // Not supported yet
        [TestCase("templates/azuresqlmi.json", 0.0, 0.0)] // Not supported yet
        [TestCase("templates/mysqldb.json", 0.0, 0.0)] // Not supported yet
        [TestCase("templates/postgresql.json", 10190.492299999996d, 10190.492299999996d)]
        [TestCase("templates/servicebus.json", 688.50693000000001d, 688.50693000000001d)]
        [TestCase("templates/datafactory.json", 189.13086000000001d, 189.13086000000001d)]
        [TestCase("templates/appgw.json", 1598.7849000000001d, 1598.7849000000001d)]
        public async Task ResourceEstimation_ShouldBeCalculatedCorrectly(string templatePath, double totalValue, double deltaValue)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                templatePath, 
                "f81e70a7-e819-49b2-a980-8e9c433743dd",
                "arm-estimator-rg", 
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
            Assert.That(output.TotalCost, Is.EqualTo(totalValue));
            Assert.That(output.Delta, Is.EqualTo(deltaValue));
        }
    }
}