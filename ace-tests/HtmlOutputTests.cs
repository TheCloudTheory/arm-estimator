// using ACE;

// namespace arm_estimator_tests
// {
//     [Ignore("Under rework")]
//     internal class HtmlOutputTests
//     {
//         [Test]
//         public async Task WhenHtmlOutputOptionIsEnabled_ItShouldGenerateOutputFile()
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/output.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateHtmlOutput",
//                 "--inline",
//                 $"acrName=acr{DateTime.Now.Ticks}",
//                 "--htmlOutputFilename",
//                 $"{outputFilename}"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.Exists($"{outputFilename}.html");
//             Assert.That(outputFile, Is.True);
//         }
//     }
// }
