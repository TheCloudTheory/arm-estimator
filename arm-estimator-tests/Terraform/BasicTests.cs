using ACE;
using System.Diagnostics;
using System.Text.Json;

namespace arm_estimator_tests.Terraform
{
    internal class BasicTests
    {
        [Test]
        [Parallelizable(ParallelScope.Self)]
        [Category("Terraform")]
        public async Task TF_WhenCalculationIsPerformed_ItShouldGiveCorrectValue()
        {
            InitializeAndCreateTerraformPlan("templates/terraform");

            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/terraform/main.tf",
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
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(5.0979999999999999d));
            Assert.That(output.TotalResourceCount, Is.EqualTo(3));
        }

        private void InitializeAndCreateTerraformPlan(string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "terraform";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.Arguments = "init";
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
                process.StartInfo.Arguments = "plan -out tfplan";
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
        }
    }
}
