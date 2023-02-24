﻿using ACE;
using System.Text.Json;

namespace arm_estimator_tests.Bicep
{
    internal class VmTests
    {
        [Test]
        [TestCase("Basic_A0", 13.139999999999999d)]
        [TestCase("Basic_A1", 26.279999999999998d)]
        [TestCase("Basic_A2", 88.33d)]
        [TestCase("Basic_A3", 236.52d)]
        [TestCase("Basic_A4", 473.04d)]
        [TestCase("Standard_A0", 14.60d)]
        [TestCase("Standard_A1", 65.70d)]
        [TestCase("Standard_A2", 131.40d)]
        [TestCase("Standard_A3", 262.80d)]
        [TestCase("Standard_A4", 525.60d)]
        [TestCase("Standard_A5", 248.20000000000002d)]
        [TestCase("Standard_A6", 496.40000000000003d)]
        [TestCase("Standard_A7", 992.80000000000007d)]
        [TestCase("Standard_A1_v2", 45.26d)]
        [TestCase("Standard_A2_v2", 94.90d)]
        [TestCase("Standard_A4_v2", 200.02d)]
        [TestCase("Standard_A8_v2", 419.74999999999994d)]
        [TestCase("Standard_A2m_v2", 137.24d)]
        [TestCase("Standard_A4m_v2", 287.62d)]
        [TestCase("Standard_A8m_v2", 603.70999999999992d)]
        [TestCase("Standard_B1s", 11.68d)]
        [TestCase("Standard_B2s", 40.88d)]
        [TestCase("Standard_B1ms", 20.221d)]
        [TestCase("Standard_B2ms", 75.92d)]
        [TestCase("Standard_B4ms", 151.84d)]
        [TestCase("Standard_B8ms", 303.68d)]
        [TestCase("Standard_D2a_v4", 151.10999999999999d)]
        [TestCase("Standard_D4a_v4", 302.21999999999997d)]
        [TestCase("Standard_D8a_v4", 604.43999999999994d)]
        [TestCase("Standard_D2ads_v5", 158.41d)]
        [TestCase("Standard_D4ads_v5", 316.82d)]
        [TestCase("Standard_D8ads_v5", 633.64d)]
        [TestCase("Standard_D2as_v4", 151.10999999999999d)]
        [TestCase("Standard_D4as_v4", 302.21999999999997d)]
        [TestCase("Standard_D8as_v4", 604.43999999999994d)]
        [TestCase("Standard_D2as_v5", 143.08d)]
        [TestCase("Standard_D4as_v5", 286.16d)]
        [TestCase("Standard_D8as_v5", 572.32d)]
        [TestCase("Standard_DC2ads_v5", 167.90d)]
        [TestCase("Standard_DC4ads_v5", 335.07d)]
        [TestCase("Standard_DC8ads_v5", 670.14d)]
        [TestCase("Standard_DC2as_v5", 150.38d)]
        [TestCase("Standard_DC4as_v5", 301.49d)]
        [TestCase("Standard_DC8as_v5", 602.98d)]
        [TestCase("Standard_DC1ds_v3", 132.85999999999999d)]
        [TestCase("Standard_DC2ds_v3", 265.71999999999997d)]
        [TestCase("Standard_DC4ds_v3", 531.43999999999994d)]
        [TestCase("Standard_DC8ds_v3", 1062.8799999999999d)]
        [TestCase("Standard_DC1s_v2", 117.53d)]
        [TestCase("Standard_DC2s_v2", 235.06d)]
        [TestCase("Standard_DC4s_v2", 470.12d)]
        [TestCase("Standard_DC8s_v2", 940.24d)]
        [TestCase("Standard_DC1s_v3", 117.53d)]
        [TestCase("Standard_DC2s_v3", 235.06d)]
        [TestCase("Standard_DC4s_v3", 470.12d)]
        [TestCase("Standard_DC8s_v3", 940.24d)]
        [TestCase("Standard_D2d_v4", 166.44d)]
        [TestCase("Standard_D4d_v4", 332.88d)]
        [TestCase("Standard_D8d_v4", 665.76d)]
        [TestCase("Standard_D2ds_v4", 166.44d)]
        [TestCase("Standard_D4ds_v4", 332.88d)]
        [TestCase("Standard_D8ds_v4", 665.76d)]
        [TestCase("Standard_D2ds_v5", 166.44d)]
        [TestCase("Standard_D4ds_v5", 332.88d)]
        [TestCase("Standard_D8ds_v5", 665.76d)]
        [TestCase("Standard_D2d_v5", 166.44d)]
        [TestCase("Standard_D4d_v5", 332.88d)]
        [TestCase("Standard_D8d_v5", 665.76d)]
        [TestCase("Standard_D1s", 108.03999999999999d)]
        [TestCase("Standard_D2s", 216.81d)]
        [TestCase("Standard_D3s", 432.89d)]
        [TestCase("Standard_D4s", 866.50999999999999d)]
        [TestCase("Standard_DS1_v2", 97.09d)]
        [TestCase("Standard_DS2_v2", 194.18d)]
        [TestCase("Standard_DS4_v2", 778.18000000000006d)]
        [TestCase("Standard_D2s_v3", 154.75999999999999d)]
        [TestCase("Standard_D4s_v3", 309.52d)]
        [TestCase("Standard_D8s_v3", 619.04d)]
        [TestCase("Standard_D2s_v4", 151.10999999999999d)]
        [TestCase("Standard_D4s_v4", 302.21999999999997d)]
        [TestCase("Standard_D8s_v4", 604.43999999999994d)]
        [TestCase("Standard_D2s_v5", 151.10999999999999d)]
        [TestCase("Standard_D4s_v5", 302.21999999999997d)]
        [TestCase("Standard_D8s_v5", 604.43999999999994d)]
        [TestCase("Standard_D1_v2", 97.09d)]
        [TestCase("Standard_D2_v2", 194.18d)]
        [TestCase("Standard_D3_v2", 389.09000000000003d)]
        [TestCase("Standard_D4_v2", 778.18000000000006d)]
        [TestCase("Standard_D2_v3", 154.76d)]
        [TestCase("Standard_D4_v3", 309.52d)]
        [TestCase("Standard_D8_v3", 619.04d)]
        [TestCase("Standard_D2_v4", 151.10999999999999d)]
        [TestCase("Standard_D4_v4", 302.21999999999997d)]
        [TestCase("Standard_D8_v4", 604.43999999999994d)]
        [TestCase("Standard_D2_v5", 151.10999999999999d)]
        [TestCase("Standard_D4_v5", 302.21999999999997d)]
        [TestCase("Standard_D8_v5", 604.43999999999994d)]
        [Parallelizable(ParallelScope.All)]
        public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachine_Windows(string vmSize, double totalValue)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/vm/vm-windows.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"vmSize={vmSize}",
                "--inline",
                $"vmName={vmSize}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(totalValue));
        }

        [Test]
        [TestCase("Standard_A1_v2", 18.25d)]
        [TestCase("Standard_A2_v2", 37.960000000000001d)]
        [TestCase("Standard_A2m_v2", 54.75d)]
        [TestCase("Standard_D2a_v4", 60.443999999999996d)]
        [TestCase("Standard_D2ads_v5", 63.364000000000004d)]
        [TestCase("Standard_D2as_v4", 60.443999999999996d)]
        [TestCase("Standard_D2as_v5", 57.231999999999999d)]
        [TestCase("Standard_DC2ads_v5", 67.01400000000001d)]
        [TestCase("Standard_DC2as_v5", 60.298000000000002d)]
        [TestCase("Standard_DC1ds_v3", 53.144000000000005d)]
        [TestCase("Standard_DC2ds_v3", 106.58d)]
        [TestCase("Standard_DC1s_v2", 47.012d)]
        [TestCase("Standard_DC2s_v2", 94.170000000000002d)]
        [TestCase("Standard_DC1s_v3", 47.012d)]
        [TestCase("Standard_DC2s_v3", 94.170000000000002d)]
        [TestCase("Standard_D2d_v4", 66.576000000000008d)]
        [TestCase("Standard_D2ds_v4", 66.576000000000008d)]
        [TestCase("Standard_D2ds_v5", 66.576000000000008d)]
        [TestCase("Standard_D2d_v5", 66.576000000000008d)]
        [TestCase("Standard_D1s", 43.07d)]
        [TestCase("Standard_D2s", 86.86999999999999d)]
        [TestCase("Standard_DS1_v2", 38.908999999999999d)]
        [TestCase("Standard_DS2_v2", 78.109999999999999d)]
        [TestCase("Standard_D2s_v3", 61.904000000000003d)]
        [TestCase("Standard_D2s_v4", 60.443999999999996d)]
        [TestCase("Standard_D2s_v5", 60.443999999999996d)]
        [TestCase("Standard_D1_v2", 38.908999999999999d)]
        [TestCase("Standard_D2_v2", 78.109999999999999d)]
        [TestCase("Standard_D2_v3", 61.904000000000003d)]
        [TestCase("Standard_D2_v4", 60.443999999999996d)]
        [TestCase("Standard_D2_v5", 60.443999999999996d)]
        [Parallelizable(ParallelScope.All)]
        public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachineLowPriority_Windows(string vmSize, double totalValue)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/vm/vm-windows-low.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"vmSize={vmSize}",
                "--inline",
                $"vmName={vmSize}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(totalValue));
        }

        [Test]
        [TestCase("Basic_A0", 13.139999999999999d)]
        [TestCase("Basic_A1", 19.71d)]
        [TestCase("Basic_A2", 56.94d)]
        [TestCase("Basic_A3", 148.92d)]
        [TestCase("Basic_A4", 297.84d)]
        [TestCase("Standard_A0", 14.60d)]
        [TestCase("Standard_A1", 43.80d)]
        [TestCase("Standard_A2", 87.60d)]
        [TestCase("Standard_A3", 175.20d)]
        [TestCase("Standard_A4", 350.40d)]
        [TestCase("Standard_A5", 197.10000000000002d)]
        [TestCase("Standard_A6", 394.20000000000005d)]
        [TestCase("Standard_A7", 788.40000000000009d)]
        [TestCase("Standard_A1_v2", 29.93d)]
        [TestCase("Standard_A2_v2", 63.509999999999998d)]
        [TestCase("Standard_A4_v2", 133.59d)]
        [TestCase("Standard_A8_v2", 279.59000000000003d)]
        [TestCase("Standard_A2m_v2", 90.519999999999996d)]
        [TestCase("Standard_A4m_v2", 189.80000000000001d)]
        [TestCase("Standard_A8m_v2", 398.58000000000004d)]
        [TestCase("Standard_B1s", 8.7599999999999998d)]
        [TestCase("Standard_B2s", 35.039999999999999d)]
        [TestCase("Standard_B1ls", 4.3799999999999999d)]
        [TestCase("Standard_B1ms", 17.52d)]
        [TestCase("Standard_B2ms", 70.079999999999998d)]
        [TestCase("Standard_B4ms", 140.16d)]
        [TestCase("Standard_B8ms", 280.31999999999999d)]
        [TestCase("Standard_D2a_v4", 83.950000000000003d)]
        [TestCase("Standard_D4a_v4", 167.90000000000001d)]
        [TestCase("Standard_D8a_v4", 335.80000000000001d)]
        [TestCase("Standard_D2ads_v5", 91.25d)]
        [TestCase("Standard_D4ads_v5", 182.5d)]
        [TestCase("Standard_D8ads_v5", 365.0d)]
        [TestCase("Standard_D2as_v4", 83.950000000000003d)]
        [TestCase("Standard_D4as_v4", 167.90000000000001d)]
        [TestCase("Standard_D8as_v4", 335.80000000000001d)]
        [TestCase("Standard_D2as_v5", 75.920000000000002d)]
        [TestCase("Standard_D4as_v5", 151.84d)]
        [TestCase("Standard_D8as_v5", 303.68000000000001d)]
        [TestCase("Standard_DC2ads_v5", 100.74000000000001d)]
        [TestCase("Standard_DC4ads_v5", 200.75000000000003d)]
        [TestCase("Standard_DC8ads_v5", 401.50000000000006d)]
        [TestCase("Standard_DC2as_v5", 83.219999999999999d)]
        [TestCase("Standard_DC4as_v5", 167.17000000000002d)]
        [TestCase("Standard_DC8as_v5", 334.34000000000003d)]
        [TestCase("Standard_DC1ds_v3", 99.280000000000001d)]
        [TestCase("Standard_DC2ds_v3", 198.56d)]
        [TestCase("Standard_DC4ds_v3", 397.12d)]
        [TestCase("Standard_DC8ds_v3", 794.24d)]
        [TestCase("Standard_DC1s_v2", 83.950000000000003d)]
        [TestCase("Standard_DC2s_v2", 167.90000000000001d)]
        [TestCase("Standard_DC4s_v2", 335.80000000000001d)]
        [TestCase("Standard_DC8s_v2", 671.60000000000002d)]
        [TestCase("Standard_DC1s_v3", 83.950000000000003d)]
        [TestCase("Standard_DC2s_v3", 167.90000000000001d)]
        [TestCase("Standard_DC4s_v3", 335.80000000000001d)]
        [TestCase("Standard_DC8s_v3", 671.60000000000002d)]
        [TestCase("Standard_D2d_v4", 99.280000000000001d)]
        [TestCase("Standard_D4d_v4", 198.56d)]
        [TestCase("Standard_D8d_v4", 397.12d)]
        [TestCase("Standard_D2ds_v4", 99.280000000000001d)]
        [TestCase("Standard_D4ds_v4", 198.56d)]
        [TestCase("Standard_D8ds_v4", 397.12d)]
        [TestCase("Standard_D2ds_v5", 99.280000000000001d)]
        [TestCase("Standard_D4ds_v5", 198.56d)]
        [TestCase("Standard_D8ds_v5", 397.12d)]
        [TestCase("Standard_D2d_v5", 99.280000000000001d)]
        [TestCase("Standard_D4d_v5", 198.56d)]
        [TestCase("Standard_D8d_v5", 397.12d)]
        [TestCase("Standard_D1s", 61.32d)]
        [TestCase("Standard_D2s", 122.64d)]
        [TestCase("Standard_D3s", 245.28d)]
        [TestCase("Standard_D4s", 490.56d)]
        [TestCase("Standard_DS1_v2", 49.567d)]
        [TestCase("Standard_DS2_v2", 99.280000000000001d)]
        [TestCase("Standard_DS3_v2", 198.56d)]
        [TestCase("Standard_DS4_v2", 397.12d)]
        [TestCase("Standard_D2s_v3", 87.599999999999994d)]
        [TestCase("Standard_D4s_v3", 175.19999999999999d)]
        [TestCase("Standard_D8s_v3", 350.39999999999998d)]
        [TestCase("Standard_D2s_v4", 83.950000000000003d)]
        [TestCase("Standard_D4s_v4", 167.90000000000001d)]
        [TestCase("Standard_D8s_v4", 335.80000000000001d)]
        [TestCase("Standard_D2s_v5", 83.950000000000003d)]
        [TestCase("Standard_D4s_v5", 167.90000000000001d)]
        [TestCase("Standard_D8s_v5", 335.80000000000001d)]
        [TestCase("Standard_D1_v2", 49.567d)]
        [TestCase("Standard_D2_v2", 99.280000000000001d)]
        [TestCase("Standard_D3_v2", 198.56d)]
        [TestCase("Standard_D4_v2", 397.12d)]
        [TestCase("Standard_D2_v3", 87.599999999999994d)]
        [TestCase("Standard_D4_v3", 175.19999999999999d)]
        [TestCase("Standard_D8_v3", 350.39999999999998d)]
        [TestCase("Standard_D2_v4", 83.950000000000003d)]
        [TestCase("Standard_D4_v4", 167.90000000000001d)]
        [TestCase("Standard_D8_v4", 335.80000000000001d)]
        [TestCase("Standard_D2_v5", 83.950000000000003d)]
        [TestCase("Standard_D4_v5", 167.90000000000001d)]
        [TestCase("Standard_D8_v5", 335.80000000000001d)]
        [Parallelizable(ParallelScope.All)]
        public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachineLinux(string vmSize, double totalValue)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/vm/vm-linux.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"vmSize={vmSize}",
                "--inline",
                $"vmName={vmSize}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(totalValue));
        }

        [Test]
        [TestCase("Standard_A1_v2", 5.8399999999999999d)]
        [TestCase("Standard_A2_v2", 12.41d)]
        [TestCase("Standard_A2m_v2", 18.25d)]
        [TestCase("Standard_D2a_v4", 16.789999999999999d)]
        [TestCase("Standard_D2ads_v5", 18.25d)]
        [TestCase("Standard_D2as_v4", 16.789999999999999d)]
        [TestCase("Standard_D2as_v5", 15.183999999999999d)]
        [TestCase("Standard_DC2ads_v5", 20.074999999999999d)]
        [TestCase("Standard_DC2as_v5", 16.716999999999999d)]
        [TestCase("Standard_DC1ds_v3", 19.855999999999998d)]
        [TestCase("Standard_DC2ds_v3", 39.711999999999996d)]
        [TestCase("Standard_DC1s_v2", 16.789999999999999d)]
        [TestCase("Standard_DC2s_v2", 33.579999999999998d)]
        [TestCase("Standard_DC1s_v3", 16.789999999999999d)]
        [TestCase("Standard_DC2s_v3", 33.579999999999998d)]
        [TestCase("Standard_D2d_v4", 19.855999999999998d)]
        [TestCase("Standard_D2ds_v4", 19.855999999999998d)]
        [TestCase("Standard_D2ds_v5", 19.855999999999998d)]
        [TestCase("Standard_D2d_v5", 19.855999999999998d)]
        [TestCase("Standard_D1s", 12.263999999999999d)]
        [TestCase("Standard_D2s", 24.527999999999999d)]
        [TestCase("Standard_DS1_v2", 9.927999999999999d)]
        [TestCase("Standard_DS2_v2", 19.710000000000001d)]
        [TestCase("Standard_D2s_v3", 17.52d)]
        [TestCase("Standard_D2s_v4", 16.789999999999999d)]
        [TestCase("Standard_D2s_v5", 16.789999999999999d)]
        [TestCase("Standard_D1_v2", 9.927999999999999d)]
        [TestCase("Standard_D2_v2", 19.710000000000001d)]
        [TestCase("Standard_D2_v3", 17.52d)]
        [TestCase("Standard_D2_v4", 16.789999999999999d)]
        [TestCase("Standard_D2_v5", 16.789999999999999d)]
        [Parallelizable(ParallelScope.All)]
        public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachineLowPriority_Linux(string vmSize, double totalValue)
        {
            var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
            var exitCode = await Program.Main(new[] {
                "templates/bicep/vm/vm-linux-low.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--generateJsonOutput",
                "--jsonOutputFilename",
                outputFilename,
                "--inline",
                $"vmSize={vmSize}",
                "--inline",
                $"vmName={vmSize}"
            });

            Assert.That(exitCode, Is.EqualTo(0));

            var outputFile = File.ReadAllText($"{outputFilename}.json");
            var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.That(output, Is.Not.Null);
            Assert.That(output.TotalCost.OriginalValue, Is.EqualTo(totalValue));
        }
    }
}
