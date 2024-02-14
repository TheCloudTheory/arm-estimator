using ACE;
using System.Text.Json;

namespace arm_estimator_tests.ARM
{
    [Ignore("Under rework")]
    internal class AksTests
    {
        // [Test]
        // [Parallelizable(ParallelScope.Self)]
        // public async Task Aks_Bug149_ShouldBeCalculatedCorrectly()
        // {
        //     var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
        //     var exitCode = await Program.Main(new[] {
        //         "templates/aks/149-bug.json",
        //         "cf70b558-b930-45e4-9048-ebcefb926adf",
        //         "arm-estimator-tests-rg",
        //         "--parameters",
        //         "templates/aks/149-bug.parameters.json",
        //         "--generateJsonOutput",
        //         "--jsonOutputFilename",
        //         outputFilename
        //     });

        //     Assert.That(exitCode, Is.EqualTo(0));

        //     var outputFile = File.ReadAllText($"{outputFilename}.json");
        //     var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
        //     {
        //         PropertyNameCaseInsensitive = true
        //     });

        //     Assert.That(output, Is.Not.Null);
        //     Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(945.54459999999926d));
        // }
    }
}
