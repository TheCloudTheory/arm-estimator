namespace ACE_Tests.Reworked.KeyVault;

public class KeyVaultWithUdfTests
{
    [Test]
    public void KeyVault_Udf_WhenThereIsAzureKeyVaultUsagePattern1_ItShouldBeInferredForCalculation()
    {
        var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        var exitCode = Program.Main([
            "templates/reworked/key-vault/kv-with-udf.bicep",
            "cf70b558-b930-45e4-9048-ebcefb926adf",
            "arm-estimator-tests-rg",
            "--generate-json-output",
            "--json-output-filename",
            outputFilename,
            "--debug",
            "--mocked-retail-api-response-path",
            "mocked-responses/retail-api/key-vault/usage-patterns.json"
        ]);

        Assert.That(exitCode, Is.EqualTo(0));

        var outputFile = File.ReadAllText($"{outputFilename}.json");
        var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, Shared.JsonSerializerOptions);

        Assert.That(output, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(1104.80d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(1));
        });
    }
}