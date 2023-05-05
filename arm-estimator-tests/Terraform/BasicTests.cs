using ACE;
using System.Diagnostics;
using System.Text.Json;

namespace arm_estimator_tests.Terraform
{
    internal class BasicTests
    {
        [Test]
        [TestCase("templates/terraform", 5.0979999999999999d, 3)]
        [TestCase("templates/terraform/aks", 108.88d, 2)]
        [TestCase("templates/terraform/analysisservice", 591.30000000000007d, 2)]
        [TestCase("templates/terraform/apim", 48.033999999999999d, 2)]
        [TestCase("templates/terraform/appconfiguration", 2.5215999999999998d, 3)]
        [TestCase("templates/terraform/applicationgateway", 40.888000000000005d, 6)]
        [TestCase("templates/terraform/applicationinsights", 5.4199999999999999d, 2)]
        [TestCase("templates/terraform/appservice", 73.0d, 3)]
        [TestCase("templates/terraform/asr", 56.860759999999999d, 20)]
        [Parallelizable(ParallelScope.All)]
        [Category("Terraform")]
        public async Task TF_WhenCalculationIsPerformed_ItShouldGiveCorrectValue(string path, double cost, int numberOfResources)
        {
            InitializeAndCreateTerraformPlan(path);

            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                $"{path}/main.tf",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(cost));
            Assert.That(output.TotalResourceCount, Is.EqualTo(numberOfResources));
        }

        private void InitializeAndCreateTerraformPlan(string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "terraform";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.Arguments = "init -no-color";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WorkingDirectory = workingDirectory;

                string? error = null;
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });

                process.Start();
                process.BeginErrorReadLine();
                process.WaitForExit();

                Assert.That(string.IsNullOrEmpty(error), Is.True);
            }

            using (var process = new Process())
            {
                process.StartInfo.FileName = "terraform";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.Arguments = "plan -out tfplan -no-color";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WorkingDirectory = workingDirectory;

                string? error = null;
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                Assert.That(string.IsNullOrEmpty(error), Is.True);
            }
        }
    }
}
