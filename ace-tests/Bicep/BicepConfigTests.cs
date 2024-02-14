// using ACE;

// namespace arm_estimator_tests.Bicep;

// [Ignore("Under rework")]
// public class BicepConfigTests
// {
//     [Test]
//     [Parallelizable(ParallelScope.Self)]
//     public async Task WhenBicepConfigIsFound_InformationShouldBePutInConsoleOutput()
//     {
//         var logFilename = $"ace_test_{DateTime.Now.Ticks}";
//         var exitCode = await Program.Main(new[] {
//                 "templates/bicep/config/infra.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--log-file",
//                 logFilename
//             });

//         Assert.Multiple(() =>
//         {
//             Assert.That(exitCode, Is.EqualTo(0));
//             Assert.That(File.Exists(logFilename), Is.True);
//             Assert.That(File.ReadAllText(logFilename), Does.Contain("Found configuration file 'bicepconfig.json' in the current directory."));
//         });
//     }
// }