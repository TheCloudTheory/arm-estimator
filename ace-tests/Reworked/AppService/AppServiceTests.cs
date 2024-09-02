
namespace ACE_Tests.Reworked.AppService;

[Parallelizable(ParallelScope.Self)]
public class AppServiceTests
{
    [Test]
    [TestCase("F1", 0.00d)]
    [TestCase("P0V3", 64.97d)]
    public void AppService_Sku_WhenAppServicePlanWithGivenSKUIsUsed_ItShouldBeSupported(string sku, double cost)
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = Program.Main([
                    "templates/reworked/app-service/skus.bicep",
                    "cf70b558-b930-45e4-9048-ebcefb926adf",
                    "arm-estimator-tests-rg",
                    "--generate-json-output",
                    "--json-output-filename",
                    outputFilename,
                    "--debug",
                    "--inline",
                    $"paramSkuName={sku}",
                ]);

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(2));
        });
    }
}
