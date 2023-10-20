using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Diagnostics;

namespace ACE.Compilation
{
    internal class BicepCompiler : ICompiler
    {
        private readonly ILogger<Program> logger;

        public BicepCompiler(ILogger<Program> logger)
        {
            this.logger = logger;
        }

        public string? Compile(FileInfo templateFile, CancellationToken token)
        {
            string? template;

            try
            {
                if(token.IsCancellationRequested)
                {
                    return null;
                }

                this.logger.AddEstimatorMessage("Attempting to compile Bicep file using Bicep CLI.");
                CompileBicepWith("bicep", $"build {templateFile} --stdout", token, logger, out template);
            }
            catch (Win32Exception)
            {
                // First compilation may not work if Bicep CLI is not installed directly,
                // try to use Azure CLI instead
                this.logger.AddEstimatorMessage("Compilation failed, attempting to compile Bicep file using Azure CLI.");
                CompileBicepWith("az", $"bicep build --file {templateFile} --stdout", token, logger, out template);
            }

            return template;
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
