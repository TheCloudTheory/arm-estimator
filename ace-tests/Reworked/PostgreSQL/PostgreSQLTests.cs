namespace ACE_Tests.Reworked.PostgreSQL;

[Parallelizable(ParallelScope.Self)]
public class PostgreSQLTests
{
    [Test]
    public void PostgreSQL_Basic_WhenThereIsBurstableSKU_ItShouldNotThrowExceptionAndGiveCorrectEstimation()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = Program.Main([
                    "templates/reworked/postgresql/fix-261.bicep",
                    "cf70b558-b930-45e4-9048-ebcefb926adf",
                    "arm-estimator-tests-rg",
                    "--generate-json-output",
                    "--json-output-filename",
                    outputFilename,
                    "--debug",
                    "--mocked-retail-api-response-path",
                    "mocked-responses/retail-api/postgresql/burstable.json"
                ]);

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(14.990300000000001d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        });
    }
}
