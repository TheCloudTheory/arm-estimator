using System.Text.Json;

namespace arm_estimator_tests
{
    public class Generic_Bicep_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("Basic_A0", 13.139999999999999d)]
        [TestCase("Basic_A1", 26.279999999999998d)]
        [TestCase("Basic_A2", 88.33d)]
        [TestCase("Basic_A3", 236.52d)]
        [TestCase("Basic_A4", 473.04d)]
        [TestCase("Standard_A0", 14.60d)]
        [TestCase("Standard_A1", 65.70d)]
        [TestCase("Standard_A2", 131.40d)]
        [TestCase("Standard_A3", 262.80d)]
        [TestCase("Standard_A4", 525.60d)]
        [TestCase("Standard_A5", 248.20d)]
        [TestCase("Standard_A6", 496.40d)]
        [TestCase("Standard_A7", 992.80d)]
        public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachine_Windows(string vmSize, double totalValue)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/vm/vm-windows.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg", 
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"vmSize={vmSize}",
                "--inline",
                $"vmName={vmSize}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost, Is.EqualTo(totalValue));
        }
    }
}