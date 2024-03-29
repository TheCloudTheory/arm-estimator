﻿// using ACE;
// using System.Text.Json;

// namespace arm_estimator_tests.Bicep
// {
//     [Ignore("Under rework")]
//     internal class VmTests
//     {
//         [Test]
//         [Ignore("This test is ignored because it requires redesigning.")]
//         [TestCase("Basic_A0", 22.739999999999998d)]
//         [TestCase("Basic_A1", 35.879999999999995d)]
//         [TestCase("Basic_A2", 97.929999999999993d)]
//         [TestCase("Basic_A3", 246.12d)]
//         [TestCase("Basic_A4", 482.64000000000004d)]
//         [TestCase("Standard_A0", 24.199999999999999d)]
//         [TestCase("Standard_A1", 75.299999999999997d)]
//         [TestCase("Standard_A1_v2", 54.859999999999999d)]
//         [TestCase("Standard_A2", 141.0d)]
//         [TestCase("Standard_A2_v2", 104.5d)]
//         [TestCase("Standard_A2m_v2", 146.84d)]
//         [TestCase("Standard_A3", 272.40000000000003d)]
//         [TestCase("Standard_A4", 535.20000000000005d)]
//         [TestCase("Standard_A4_v2", 209.62d)]
//         [TestCase("Standard_A4m_v2", 297.22000000000003d)]
//         [TestCase("Standard_A5", 257.80000000000001d)]
//         [TestCase("Standard_A6", 506.00000000000006d)]
//         [TestCase("Standard_A7", 1002.4000000000001d)]
//         [TestCase("Standard_A8_v2", 429.34999999999997d)]
//         [TestCase("Standard_A8m_v2", 613.30999999999995d)]
//         [TestCase("Standard_B1ms", 41.900999999999996d)]
//         [TestCase("Standard_B1s", 33.359999999999999d)]
//         [TestCase("Standard_B2ms", 97.599999999999994d)]
//         [TestCase("Standard_B2s", 62.560000000000002d)]
//         [TestCase("Standard_B4ms", 173.52000000000001d)]
//         [TestCase("Standard_B8ms", 325.36000000000001d)]
//         [TestCase("Standard_D1_v2", 106.69d)]
//         [TestCase("Standard_D1s", 108.03999999999999d)]
//         [TestCase("Standard_D2_v2", 203.78d)]
//         [TestCase("Standard_D2_v3", 164.35999999999999d)]
//         [TestCase("Standard_D2_v4", 160.70999999999998d)]
//         [TestCase("Standard_D2_v5", 160.70999999999998d)]
//         [TestCase("Standard_D2a_v4", 160.70999999999998d)]
//         [TestCase("Standard_D2ads_v5", 180.09d)]
//         [TestCase("Standard_D2as_v4", 172.78999999999999d)]
//         [TestCase("Standard_D2as_v5", 164.76000000000002d)]
//         [TestCase("Standard_D2d_v4", 176.03999999999999d)]
//         [TestCase("Standard_D2d_v5", 176.03999999999999d)]
//         [TestCase("Standard_D2ds_v4", 188.12d)]
//         [TestCase("Standard_D2ds_v5", 188.12d)]
//         [TestCase("Standard_D2s", 216.81d)]
//         [TestCase("Standard_D2s_v3", 176.44d)]
//         [TestCase("Standard_D2s_v4", 172.78999999999999d)]
//         [TestCase("Standard_D2s_v5", 172.78999999999999d)]
//         [TestCase("Standard_D3_v2", 398.69000000000005d)]
//         [TestCase("Standard_D3s", 432.89d)]
//         [TestCase("Standard_D4s", 866.50999999999999d)]
//         [TestCase("Standard_D4_v2", 787.78000000000009d)]
//         [TestCase("Standard_D4_v3", 319.12d)]
//         [TestCase("Standard_D4_v4", 311.81999999999999d)]
//         [TestCase("Standard_D4_v5", 311.81999999999999d)]
//         [TestCase("Standard_D4a_v4", 311.81999999999999d)]
//         [TestCase("Standard_D4ads_v5", 338.5d)]
//         [TestCase("Standard_D4as_v4", 323.89999999999998d)]
//         [TestCase("Standard_D4as_v5", 307.84000000000003d)]
//         [TestCase("Standard_DC4ads_v5", 338.5d)]
//         [TestCase("Standard_DC4as_v5", 307.84000000000003d)]
//         [TestCase("Standard_DC4ds_v3", 553.11999999999989d)]
//         [TestCase("Standard_DC4s_v2", 491.80000000000001d)]
//         [TestCase("Standard_D8_v3", 628.63999999999999d)]
//         [TestCase("Standard_D8_v4", 614.03999999999996d)]
//         [TestCase("Standard_D8_v5", 614.03999999999996d)]
//         [TestCase("Standard_D8a_v4", 614.03999999999996d)]
//         [TestCase("Standard_D8ads_v5", 655.31999999999994d)]
//         [TestCase("Standard_D8as_v4", 626.11999999999989d)]
//         [TestCase("Standard_D8as_v5", 594.0d)]
//         [TestCase("Standard_DC2ads_v5", 180.09d)]
//         [TestCase("Standard_DC8ads_v5", 655.31999999999994d)]
//         [TestCase("Standard_DC2as_v5", 164.76000000000002d)]
//         [TestCase("Standard_DC8as_v5", 594.0d)]
//         [TestCase("Standard_DC1ds_v3", 154.53999999999999d)]
//         [TestCase("Standard_DC2ds_v3", 287.39999999999998d)]
//         [TestCase("Standard_DC8ds_v3", 1084.5599999999999d)]
//         [TestCase("Standard_DC1s_v2", 139.21000000000001d)]
//         [TestCase("Standard_DC2s_v2", 256.74000000000001d)]
//         [TestCase("Standard_DC8s_v2", 940.24d)]
//         [TestCase("Standard_DC1s_v3", 139.21000000000001d)]
//         [TestCase("Standard_DC2s_v3", 256.74000000000001d)]
//         [TestCase("Standard_DC4s_v3", 491.80000000000001d)]
//         [TestCase("Standard_DC8s_v3", 961.91999999999996d)]
//         [TestCase("Standard_D4d_v4", 342.48000000000002d)]
//         [TestCase("Standard_D8d_v4", 675.36000000000001d)]
//         [TestCase("Standard_D4ds_v4", 354.56d)]
//         [TestCase("Standard_D8ds_v4", 687.43999999999994d)]
//         [TestCase("Standard_D4ds_v5", 354.56d)]
//         [TestCase("Standard_D8ds_v5", 687.43999999999994d)]
//         [TestCase("Standard_D4d_v5", 342.48000000000002d)]
//         [TestCase("Standard_D8d_v5", 675.36000000000001d)]
//         [TestCase("Standard_DS1_v2", 118.77000000000001d)]
//         [TestCase("Standard_DS2_v2", 215.86000000000001d)]
//         [TestCase("Standard_DS4_v2", 799.86000000000001d)]
//         [TestCase("Standard_D4s_v3", 331.19999999999999d)]
//         [TestCase("Standard_D8s_v3", 640.71999999999991d)]
//         [TestCase("Standard_D4s_v4", 323.89999999999998d)]
//         [TestCase("Standard_D8s_v4", 626.11999999999989d)]
//         [TestCase("Standard_D4s_v5", 323.89999999999998d)]
//         [TestCase("Standard_D8s_v5", 626.11999999999989d)]
//         [Parallelizable(ParallelScope.All)]
//         public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachine_Windows(string vmSize, double totalValue)
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/vm/vm-windows.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"vmSize={vmSize}",
//                 "--inline",
//                 $"vmName={vmSize}"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.That(output!.TotalCost.OriginalValue, Is.EqualTo(totalValue));
//         }

//         [Test]
//         [Ignore("This test is ignored because it requires redesigning.")]
//         [TestCase("Standard_A1_v2", 27.850000000000001d)]
//         [TestCase("Standard_A2_v2", 47.560000000000002d)]
//         [TestCase("Standard_A2m_v2", 64.349999999999994d)]
//         [TestCase("Standard_D2a_v4", 70.043999999999997d)]
//         [TestCase("Standard_D2ads_v5", 85.044000000000011d)]
//         [TestCase("Standard_D2as_v4", 82.123999999999995d)]
//         [TestCase("Standard_D2as_v5", 78.912000000000006d)]
//         [TestCase("Standard_DC2ads_v5", 85.044000000000011d)]
//         [TestCase("Standard_DC2as_v5", 78.912000000000006d)]
//         [TestCase("Standard_DC1ds_v3", 74.824000000000012d)]
//         [TestCase("Standard_DC2ds_v3", 128.25999999999999d)]
//         [TestCase("Standard_DC1s_v2", 68.692000000000007d)]
//         [TestCase("Standard_DC2s_v2", 115.84999999999999d)]
//         [TestCase("Standard_DC1s_v3", 68.692000000000007d)]
//         [TestCase("Standard_DC2s_v3", 115.84999999999999d)]
//         [TestCase("Standard_D2d_v4", 76.176000000000002d)]
//         [TestCase("Standard_D2ds_v4", 88.256d)]
//         [TestCase("Standard_D2ds_v5", 88.256d)]
//         [TestCase("Standard_D2d_v5", 76.176000000000002d)]
//         [TestCase("Standard_D1s", 43.07d)]
//         [TestCase("Standard_D2s", 86.86999999999999d)]
//         [TestCase("Standard_DS1_v2", 60.588999999999999d)]
//         [TestCase("Standard_DS2_v2", 99.789999999999992d)]
//         [TestCase("Standard_D2s_v3", 83.584000000000003d)]
//         [TestCase("Standard_D2s_v4", 82.123999999999995d)]
//         [TestCase("Standard_D2s_v5", 82.123999999999995d)]
//         [TestCase("Standard_D1_v2", 48.509d)]
//         [TestCase("Standard_D2_v2", 87.709999999999994d)]
//         [TestCase("Standard_D2_v3", 71.504000000000005d)]
//         [TestCase("Standard_D2_v4", 70.043999999999997d)]
//         [TestCase("Standard_D2_v5", 70.043999999999997d)]
//         [Parallelizable(ParallelScope.All)]
//         public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachineLowPriority_Windows(string vmSize, double totalValue)
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/vm/vm-windows-low.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"vmSize={vmSize}",
//                 "--inline",
//                 $"vmName={vmSize}"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.That(output!.TotalCost.OriginalValue, Is.EqualTo(totalValue));
//         }

//         [Test]
//         [Ignore("This test is ignored because it requires redesigning.")]
//         [TestCase("Basic_A0", 22.739999999999998d)]
//         [TestCase("Standard_A0", 24.199999999999999d)]
//         [TestCase("Standard_A1_v2", 39.530000000000001d)]
//         [TestCase("Standard_A2m_v2", 100.11999999999999d)]
//         [TestCase("Standard_B1s", 30.439999999999998d)]
//         [TestCase("Standard_B1ms", 39.200000000000003d)]
//         [TestCase("Standard_D2a_v4", 93.549999999999997d)]
//         [TestCase("Standard_D2ads_v5", 112.93000000000001d)]
//         [TestCase("Standard_D2as_v4", 105.63d)]
//         [TestCase("Standard_DC2ads_v5", 112.93000000000001d)]
//         [TestCase("Standard_DC2as_v5", 97.599999999999994d)]
//         [TestCase("Standard_DC1ds_v3", 120.96000000000001d)]
//         [TestCase("Standard_DC1s_v2", 105.63d)]
//         [TestCase("Standard_DC1s_v3", 105.63d)]
//         [TestCase("Standard_D2d_v4", 108.88d)]
//         [TestCase("Standard_D2ds_v4", 120.96000000000001d)]
//         [TestCase("Standard_D2ds_v5", 120.96000000000001d)]
//         [TestCase("Standard_D2d_v5", 108.88d)]
//         [TestCase("Standard_D1s", 61.32d)]
//         [TestCase("Standard_DS1_v2", 71.247d)]
//         [TestCase("Standard_D2s_v3", 109.28d)]
//         [TestCase("Standard_D2s_v4", 105.63d)]
//         [TestCase("Standard_D1_v2", 59.167000000000002d)]
//         [TestCase("Standard_D2_v3", 97.199999999999989d)]
//         [TestCase("Standard_D2_v4", 93.549999999999997d)]
//         [TestCase("Standard_D2_v5", 93.549999999999997d)]
//         [Parallelizable(ParallelScope.All)]
//         public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachineLinux(string vmSize, double totalValue)
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/vm/vm-linux.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"vmSize={vmSize}",
//                 "--inline",
//                 $"vmName={vmSize}"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.That(output!.TotalCost.OriginalValue, Is.EqualTo(totalValue));
//         }

//         [Test]
//         [Ignore("This test is ignored because it requires redesigning.")]
//         [TestCase("Standard_A1_v2", 15.44d)]
//         [TestCase("Standard_A2m_v2", 27.850000000000001d)]
//         [TestCase("Standard_D2a_v4", 26.390000000000001d)]
//         [TestCase("Standard_D2ads_v5", 39.93d)]
//         [TestCase("Standard_D2as_v4", 38.469999999999999d)]
//         [TestCase("Standard_DC2ads_v5", 39.93d)]
//         [TestCase("Standard_DC2as_v5", 36.863999999999997d)]
//         [TestCase("Standard_DC1ds_v3", 41.536000000000001d)]
//         [TestCase("Standard_DC2ds_v3", 61.391999999999996d)]
//         [TestCase("Standard_DC1s_v2", 38.469999999999999d)]
//         [TestCase("Standard_DC1s_v3", 38.469999999999999d)]
//         [TestCase("Standard_D2d_v4", 29.455999999999996d)]
//         [TestCase("Standard_D2ds_v4", 41.536000000000001d)]
//         [TestCase("Standard_D2ds_v5", 41.536000000000001d)]
//         [TestCase("Standard_D2d_v5", 29.455999999999996d)]
//         [TestCase("Standard_D1s", 12.263999999999999d)]
//         [TestCase("Standard_DS1_v2", 31.607999999999997d)]
//         [TestCase("Standard_D2s_v3", 39.200000000000003d)]
//         [TestCase("Standard_D2s_v5", 38.469999999999999d)]
//         [TestCase("Standard_D1_v2", 19.527999999999999d)]
//         [TestCase("Standard_D2_v3", 27.119999999999997d)]
//         [TestCase("Standard_D2_v4", 26.390000000000001d)]
//         [TestCase("Standard_D2_v5", 26.390000000000001d)]
//         [Parallelizable(ParallelScope.All)]
//         public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachineLowPriority_Linux(string vmSize, double totalValue)
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/vm/vm-linux-low.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"vmSize={vmSize}",
//                 "--inline",
//                 $"vmName={vmSize}"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.That(output!.TotalCost.OriginalValue, Is.EqualTo(totalValue));
//         }

//         [Test]
//         [Ignore("This test is ignored because it requires redesigning.")]
//         [TestCase("Standard_A1_v2", 6.5262000000000002d)]
//         [TestCase("Standard_A2m_v2", 19.789570000000001d)]
//         [TestCase("Standard_D2a_v4", 20.399850000000001d)]
//         [TestCase("Standard_D2ads_v5", 21.10868d)]
//         [TestCase("Standard_D2as_v4", 20.399850000000001d)]
//         [TestCase("Standard_D2as_v5", 19.066140000000001d)]
//         [TestCase("Standard_DC2ads_v5", 67.159999999999997d)]
//         [TestCase("Standard_DC2as_v5", 60.152000000000001d)]
//         [TestCase("Standard_DC1ds_v3", 53.144000000000005d)]
//         [TestCase("Standard_DC2ds_v3", 106.28800000000001d)]
//         [TestCase("Standard_DC1s_v2", 12.72974d)]
//         [TestCase("Standard_DC1s_v3", 47.012d)]
//         [TestCase("Standard_DC2s_v3", 94.024000000000001d)]
//         [TestCase("Standard_D2d_v4", 26.164659999999998d)]
//         [TestCase("Standard_D2ds_v4", 26.164659999999998d)]
//         [TestCase("Standard_D2ds_v5", 22.450420000000001d)]
//         [TestCase("Standard_D2d_v5", 22.450420000000001d)]
//         [TestCase("Standard_D1s", 12.17348d)]
//         [TestCase("Standard_DS1_v2", 14.440860000000001d)]
//         [TestCase("Standard_D2s_v3", 22.316099999999999d)]
//         [TestCase("Standard_D2s_v4", 21.922629999999998d)]
//         [TestCase("Standard_D2s_v5", 20.38233d)]
//         [TestCase("Standard_D1_v2", 14.440860000000001d)]
//         [TestCase("Standard_D2_v3", 22.316099999999999d)]
//         [TestCase("Standard_D2_v4", 23.754200000000001d)]
//         [TestCase("Standard_D2_v5", 20.38233d)]
//         [Parallelizable(ParallelScope.All)]
//         public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachineSpot_Windows(string vmSize, double totalValue)
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/vm/vm-windows-spot.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"vmSize={vmSize}",
//                 "--inline",
//                 $"vmName={vmSize}"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//         }

//         [Test]
//         [Ignore("This test is ignored because it requires redesigning.")]
//         [TestCase("Standard_A1_v2", 3.1397300000000001d)]
//         [TestCase("Standard_A2_v2", 6.6627099999999997d)]
//         [TestCase("Standard_A2m_v2", 9.4958400000000012d)]
//         [TestCase("Standard_D2a_v4", 8.3949999999999996d)]
//         [TestCase("Standard_D2ads_v5", 9.125d)]
//         [TestCase("Standard_D2as_v4", 8.3949999999999996d)]
//         [TestCase("Standard_D2as_v5", 7.5919999999999996d)]
//         [TestCase("Standard_DC2ads_v5", 40.295999999999999d)]
//         [TestCase("Standard_DC2as_v5", 33.288000000000004d)]
//         [TestCase("Standard_DC1ds_v3", 39.711999999999996d)]
//         [TestCase("Standard_DC2ds_v3", 79.423999999999992d)]
//         [TestCase("Standard_DC1s_v2", 8.3949999999999996d)]
//         [TestCase("Standard_DC2s_v2", 16.789999999999999d)]
//         [TestCase("Standard_DC1s_v3", 33.579999999999998d)]
//         [TestCase("Standard_DC2s_v3", 67.159999999999997d)]
//         [TestCase("Standard_D2d_v4", 11.12082d)]
//         [TestCase("Standard_D2ds_v4", 11.12082d)]
//         [TestCase("Standard_D2ds_v5", 10.85656d)]
//         [TestCase("Standard_D2d_v5", 10.85656d)]
//         [TestCase("Standard_D1s", 6.1319999999999997d)]
//         [TestCase("Standard_D2s", 12.263999999999999d)]
//         [TestCase("Standard_DS1_v2", 5.8925599999999996d)]
//         [TestCase("Standard_DS2_v2", 11.802639999999998d)]
//         [TestCase("Standard_D2s_v3", 9.1892399999999999d)]
//         [TestCase("Standard_D2s_v4", 9.4031300000000009d)]
//         [TestCase("Standard_D2s_v5", 9.1797500000000003d)]
//         [TestCase("Standard_D1_v2", 5.2238800000000003d)]
//         [TestCase("Standard_D2_v2", 10.463089999999999d)]
//         [TestCase("Standard_D2_v3", 9.1892399999999999d)]
//         [TestCase("Standard_D2_v4", 9.4031300000000009d)]
//         [TestCase("Standard_D2_v5", 9.1797500000000003d)]
//         [Parallelizable(ParallelScope.All)]
//         public async Task ResourceEstimation_ShouldBeCalculatedCorrectlyForVirtualMachineSpot_Linux(string vmSize, double totalValue)
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/vm/vm-linux-spot.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"vmSize={vmSize}",
//                 "--inline",
//                 $"vmName={vmSize}"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//         }

//         [Test]
//         [TestCase("Standard_A1_v2", 54.859999999999999d)]
//         [TestCase("Standard_B1s", 33.359999999999999d)]
//         [Parallelizable(ParallelScope.All)]
//         public async Task VM_ShouldBeCalculatedCorrectlyForVirtualMachine_WithManagedDiskInferred(string vmSize, double totalValue)
//         {
//             var outputFilename = $"ace_test_{DateTime.Now.Ticks}";
//             var exitCode = await Program.Main(new[] {
//                 "templates/bicep/vm/vm-full.bicep",
//                 "cf70b558-b930-45e4-9048-ebcefb926adf",
//                 "arm-estimator-tests-rg",
//                 "--generateJsonOutput",
//                 "--jsonOutputFilename",
//                 outputFilename,
//                 "--inline",
//                 $"vmSize={vmSize}",
//                 "--inline",
//                 $"vmName={vmSize}",
//                 "--inline",
//                 $"nicName=nic{DateTime.Now.Ticks}"
//             });

//             Assert.That(exitCode, Is.EqualTo(0));

//             var outputFile = File.ReadAllText($"{outputFilename}.json");
//             var output = JsonSerializer.Deserialize<EstimationOutput>(outputFile, new JsonSerializerOptions()
//             {
//                 PropertyNameCaseInsensitive = true
//             });

//             Assert.That(output, Is.Not.Null);
//             Assert.That(output!.TotalCost.OriginalValue, Is.EqualTo(totalValue));
//         }
//     }
// }
