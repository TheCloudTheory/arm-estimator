using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace ACE.Compilation
{
    internal class BicepCompiler(
        bool forceUsingBicepCli,
        ILogger<Program> logger
        ) : ICompiler
    {
        private readonly bool forceUsingBicepCli = forceUsingBicepCli;
        private readonly ILogger<Program> logger = logger;

        public string? Compile(FileInfo templateFile, CancellationToken token)
        {
            string? template;

            try
            {
                if (token.IsCancellationRequested)
                {
                    return null;
                }

                CheckIfBicepConfigExists(templateFile);

                if (this.forceUsingBicepCli == true)
                {
                    this.logger.AddEstimatorMessage("Force Bicep CLI compilation.");                  
                    CompileBicepWith("bicep", $"build {templateFile} --stdout", token, logger, out template);
                }
                else
                {
                    this.logger.AddEstimatorMessage("Attempting to compile Bicep file using Azure CLI.");
                    CompileBicepWith("az", $"bicep build --file {templateFile} --stdout", token, logger, out template);
                }
            }
            catch (Win32Exception)
            {
                // First compilation may not work if Azure CLI is not installed directly,
                // try to use Bicep CLI instead
                this.logger.AddEstimatorMessage("Compilation failed, attempting to compile Bicep file using Bicep CLI.");
                CompileBicepWith("bicep", $"build {templateFile} --stdout", token, logger, out template);
            }

            return template;
        }

        private void CheckIfBicepConfigExists(FileInfo templateFile)
        {
            if (templateFile.DirectoryName == null)
            {
                this.logger.LogWarning("Couldn't find directory name for template file, skipping check for 'bicepconfig.json' file.");
                return;
            }

            if (File.Exists(Path.Combine(templateFile.DirectoryName, "bicepconfig.json")))
            {
                this.logger.LogWarning("Found configuration file 'bicepconfig.json' in the current directory. This file will be used to compile the Bicep file and may affect the result. See https://github.com/TheCloudTheory/arm-estimator/issues/197 for more information.");
                return;
            }
        }

        public string? CompileBicepparam(FileInfo bicepparamFile, CancellationToken token)
        {
            string? parameters;

            try
            {
                if (token.IsCancellationRequested)
                {
                    return null;
                }

                if (this.forceUsingBicepCli == true)
                {
                    this.logger.AddEstimatorMessage("Force Bicep CLI compilation.");                  
                    CompileBicepWith("bicep", $"build-params {bicepparamFile} --stdout", token, logger, out parameters);
                }
                else
                {
                    this.logger.AddEstimatorMessage("Compiling Bicepparam file using Azure CLI.");
                    CompileBicepWith("az", $"bicep build-params --file {bicepparamFile} --stdout", token, logger, out parameters);
                }

                if (parameters == null)
                {
                    return null;
                }

                // Bicep CLI returns a JSON object with a single property "parametersJson" that contains the parameters
                var result = JsonSerializer.Deserialize<BicepparamStdoutResult>(parameters);
                return result?.ParametersJson;
            }
            catch (Win32Exception)
            {
                // First compilation may not work if Azure CLI is not installed directly,
                // try to use Bicep CLI instead
                this.logger.AddEstimatorMessage("Compilation failed, attempting to compile Bicepparam file using Bicep CLI.");
                CompileBicepWith("bicep", $"build-params {bicepparamFile} --stdout", token, logger, out parameters);
            }

            return parameters;
        }

        private static void CompileBicepWith(string fileName, string arguments, CancellationToken token, ILogger logger, out string? template)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = fileName;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();

                string? error = null;
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });

                process.Start();
                process.BeginErrorReadLine();
                template = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (token.IsCancellationRequested)
                {
                    template = null;
                    return;
                }

                if (string.IsNullOrWhiteSpace(template))
                {
                    logger.LogError("{error}", error);
                    template = null;

                    return;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(error) == false)
                    {
                        // Bicep returns warnings as errors, so if a template is generated,
                        // that most likely the case and we need to handle it
                        logger.LogWarning("{warning}", error);
                    }
                }

                logger.AddEstimatorMessage("Compilation completed!");
                logger.LogInformation("");
                logger.LogInformation("------------------------------");
                logger.LogInformation("");
            }
        }
    }
}
