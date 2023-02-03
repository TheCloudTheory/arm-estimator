using ACE;

namespace arm_estimator_tests
{
    internal class HtmlOutputTests
    {
        [Test]
        public async Task WhenHtmlOutputOptionIsEnabled_ItShouldGenerateOutputFile()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/output.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateHtmlOutput",
                "--inline",
                $"acrName=acr{DateTime.Now.Ticks}"
            });

            Assert.That(exitCode, Is.EqualTo(0));
        }
    }
}
