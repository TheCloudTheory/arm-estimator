// using ACE;
// using System.Text.Json;

// namespace arm_estimator_tests
// {
//     [Ignore("Under rework")]
//     internal class WhatIfTests
//     {
//         [Test]
//         public async Task Bug118_WhenEstimationIsCalculated_WhatIfShouldNotFail()
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/118-bug.json",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--parameters",
//                 "templates/118-bug.parameters.json",
//                 "--generateJsonOutput",
//                 "--inline", 
//                 "resourceName=Byov",
//                 "--inline",
//                 "location=southcentralus",
//                 "--inline",
//                 "byoAKSSubnetId=''",
//                 "--inline",
//                 "byoAGWSubnetId=''",
//                 "--inline",
//                 "dnsZoneId=''",
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
//             Assert.That(output!.Resources.Count(), Is.EqualTo(25));
//         }
//     }
// }
