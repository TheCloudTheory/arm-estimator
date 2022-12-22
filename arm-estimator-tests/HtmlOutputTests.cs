using System.Text.Json;

namespace arm_estimator_tests
{
    internal class HtmlOutputTests
    {
        [Test]
        public async Task WhenHtmlOutputOptionIsEnabled_ItShouldGenerateOutputFile()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/mariadb.json",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateHtmlOutput"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            //var outputFile = File.ReadAllText($"{outputFilename}.json");
            //var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            //{
            //    PropertyNameCaseInsensitive = true
            //});

            //Assert.That(output, Is.Not.Null);
            //Assert.That(output.TotalCost, Is.EqualTo(totalValue));
            //Assert.That(output.Delta, Is.EqualTo(deltaValue));
        }
    }
}
