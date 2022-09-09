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
                "f81e70a7-e819-49b2-a980-8e9c433743dd",
                "arm-estimator-rg",
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
