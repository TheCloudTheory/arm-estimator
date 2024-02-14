// using System.Text.Json;
// using arm_estimator_tests;

// namespace ACE.Tests.Bicep;

// public class KeyVaultTests
// {
//     [Test]
//     [Parallelizable(ParallelScope.Self)]
//     public async Task KeyVault_WhenCalculatingCost_SKUShouldBeCorrectlyDetected()
//     {
//         var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//         var exitCode = await Program.Main(new[] {
//                 "templates/bicep/keyvault/keyvault.bicep",
//                 Constants.AZURE_SUBSCRIPTION_ID,
//                 Constants.AZURE_RESOURCE_GROUP,
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"name=ace{DateTime.Now.Ticks}"
//             });

//         Assert.That(exitCode, Is.EqualTo(0));

//         var outputFile = File.ReadAllText($"{outputFilename}.json");
//         var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//         {
//             PropertyNameCaseInsensitive = true
//         });

//         Assert.That(output, Is.Not.Null);
//         Assert.Multiple(() =>
//         {
//             Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(5.1800000000000006d));
//             Assert.That(output.TotalResourceCount, Is.EqualTo(1));
//         });
//     }
// }