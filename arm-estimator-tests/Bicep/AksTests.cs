﻿using ACE;
using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class AksTests
    {
        [Test]
        [Parallelizable(ParallelScope.Self)]
        public async Task AKS_WhenCalculationIsPerformed_ItShouldInferOSDisk()
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/aks/aks.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"clusterName=aksaceinferosdisk",
                "--inline",
                $"dnsPrefix=ace",
                "--inline",
                $"linuxAdminUsername=ace",
                "--inline",
                $"sshRSAPublicKey=ssh-rsa AAAAB...snip...UcyupgH azureuser@linuxvm"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(327.83999999999992d));
                Assert.That(output.TotalResourceCount, Is.EqualTo(1));
            });
        }
    }
}