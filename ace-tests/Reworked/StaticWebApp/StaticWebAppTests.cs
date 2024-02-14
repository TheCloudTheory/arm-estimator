using System.Text.Json;
using ACE;

namespace ACE_Tests.Reworked.StaticWebApp
{
    [Parallelizable(ParallelScope.Self)]
    public class StaticWebAppTests
    {
        [Test]
        public void StaticWebApp_WhenFreeTierIsUsed_NoCostShouldBeReported()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = Program.Main([
                    "templates/reworked/static-web-app/static-web-app-free.bicep",
                    "cf70b558-b930-45e4-9048-ebcefb926adf",
                    "arm-estimator-tests-rg",
                    "--generateJsonOutput",
                    "--jsonOutputFilename",
                    outputFilename,
                    "--inline",
                    "parLocation=westeurope",
                    "--debug"
                ]);

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

            Assert.That(output, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0));
                Assert.That(output.TotalResourceCount, Is.EqualTo(1));
            });
        }

        [Test]
        public void StaticWebApp_WhenStandardTierIsUsed_CorrectCostShouldBeReported()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = Program.Main([
                    "templates/reworked/static-web-app/static-web-app-standard.bicep",
                    "cf70b558-b930-45e4-9048-ebcefb926adf",
                    "arm-estimator-tests-rg",
                    "--generateJsonOutput",
                    "--jsonOutputFilename",
                    outputFilename,
                    "--inline",
                    "parLocation=westeurope",
                    "--mocked-retail-api-response-path",
                    "mocked-responses/retail-api/static-web-app/standard.json",
                    "--debug"
                ]);

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

            Assert.That(output, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(9d));
                Assert.That(output.TotalResourceCount, Is.EqualTo(1));
            });
        }
    }
}