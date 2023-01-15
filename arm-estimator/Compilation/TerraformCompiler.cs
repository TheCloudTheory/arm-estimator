using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ACE.Compilation
{
    internal class TerraformCompiler : ICompiler
    {
        private readonly ILogger<Program> logger;

        public TerraformCompiler(ILogger<Program> logger)
        {
            this.logger = logger;
        }

        public string? Compile(FileInfo templateFile)
        {
            InitializeProviders(templateFile);
            BuildPlan(templateFile);

            using (var process = new Process())
            {
                process.StartInfo.FileName = "terraform";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.Arguments = $"show -json tfplan";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WorkingDirectory = templateFile.DirectoryName;

                string? error = null;
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });

                if(error != null)
                {
                    this.logger.LogError("{error}", error);
                    return null;
                }

                process.Start();
                process.BeginErrorReadLine();
                var template = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return template;
            }
        }

        private void InitializeProviders(FileInfo templateFile)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "terraform";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.Arguments = $"init";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WorkingDirectory = templateFile.DirectoryName;

                string? error = null;
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });

                if (error != null)
                {
                    this.logger.LogError("{error}", error);
                }

                process.Start();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }

        private void BuildPlan(FileInfo templateFile)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "terraform";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.Arguments = $"plan -out tfplan";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WorkingDirectory = templateFile.DirectoryName;

                string? error = null;
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });

                if (error != null)
                {
                    this.logger.LogError("{error}", error);
                }

                process.Start();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }
    }
}
