// using ACE;
// using System.Text.Json;

// namespace arm_estimator_tests.Bicep
// {
//     [Ignore("Under rework")]
//     internal class ScopeTests
//     {
//         [Test]
//         [Parallelizable(ParallelScope.Self)]
//         public async Task Scope_WhenEstimationIsPerfomedOnSubscriptionLevel_ItShouldWorkCorrectly()
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "sub",
//                 "templates/bicep/scopes/subscription.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "westeurope",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"name=rg{DateTime.Now.Ticks}",
//                 "--inline",
//                 $"acrName=acr{DateTime.Now.Ticks}",
//                 "--inline",
//                 $"location=westeurope"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.Multiple(() =>
//             {
//                 Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(5.0979999999999999d));
//                 Assert.That(output.TotalResourceCount, Is.EqualTo(2));
//             });
//         }

//         [Test]
//         [Parallelizable(ParallelScope.Self)]
//         public async Task Scope_WhenEstimationIsPerfomedOnMGLevel_ItShouldWorkCorrectly()
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "mg",
//                 "templates/bicep/scopes/management-group.bicep",
//                 "ACE",
//                 "westeurope",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.Multiple(() =>
//             {
//                 Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0));
//                 Assert.That(output.TotalResourceCount, Is.EqualTo(1));
//             });
//         }

//         [Test]
//         [Ignore("This test requires elevated access in Azure AD tenant. Run it manually with properly configured access.")]
//         [Parallelizable(ParallelScope.Self)]
//         public async Task Scope_WhenEstimationIsPerfomedOnTenantLevel_ItShouldWorkCorrectly()
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "tenant",
//                 "templates/bicep/scopes/tenant.bicep",
//                 "westeurope",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.Multiple(() =>
//             {
//                 Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(0));
//                 Assert.That(output.TotalResourceCount, Is.EqualTo(1));
//             });
//         }
//     }
// }
