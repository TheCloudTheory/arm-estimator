// using ACE;
// using System.Text.Json;

// namespace arm_estimator_tests.Bicep
// {
//     [Ignore("Under rework")]
//     internal class MariaDBTests
//     {
//         [Test]
//         [TestCase("Disabled", 41.548999999999999d)]
//         [TestCase("Enabled", 53.448999999999998d)]
//         [Parallelizable(ParallelScope.All)]
//         public async Task MariaDB_WhenCalculationIsPerformedWithBackupUsagePattern_ItShouldBeCalculatedCorrectly(string geoRedundantBackup, decimal cost)
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/usagePatterns/mariadb.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"name=maria{DateTime.Now.Ticks}",
//                 "--inline",
//                 $"geoRedundantBackup={geoRedundantBackup}"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(cost));
//             Assert.That(output.TotalResourceCount, Is.EqualTo(1));
//         }
//     }
// }
