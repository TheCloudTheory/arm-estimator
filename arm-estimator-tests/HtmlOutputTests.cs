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
                "templates/mariadb.json",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateHtmlOutput"
            });

            Assert.That(exitCode, Is.EqualTo(0));
        }
    }
}
