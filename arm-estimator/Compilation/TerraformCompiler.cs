using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace ACE.Compilation
{
    internal class TerraformCompiler : ICompiler
    {
        public struct GoString
        {
            public string msg;
            public long len;
            public GoString(string msg, long len)
            {
                this.msg = msg;
                this.len = len;
            }
        }

        private readonly ILogger<Program> logger;

#if Linux
        [DllImport("ace-terraform-parser-linux.dll")]
#endif
#if Windows
        [DllImport("ace-terraform-parser-windows.dll")]
#endif
        public static extern void GenerateParsedPlan(GoString workingDir, GoString planFile);

        public TerraformCompiler(ILogger<Program> logger)
        {
            this.logger = logger;
        }

        public string? Compile(FileInfo templateFile)
        {
            var workingDirectory = templateFile.Directory?.FullName;
            ArgumentNullException.ThrowIfNull(workingDirectory, nameof(workingDirectory));

            var planFile = $"{templateFile.Directory}{Path.DirectorySeparatorChar}tfplan";

            if(File.Exists(planFile) == false)
            {
                this.logger.LogError("Couldn't parse Terraform plan file because it doesn't exist. Make sure you have 'tfplan' file created in the Terraform directory.");
                return null;
            }

            this.logger.LogInformation("Starting parsing using embedded Go parser.");

            TerraformCompiler.GenerateParsedPlan(
                new GoString(workingDirectory, workingDirectory.Length),
                new GoString(planFile, planFile.Length));

            if(File.Exists("ace-terraform-parser.log"))
            {
                var content = File.ReadAllText("ace-terraform-parser.log");
                if(string.IsNullOrEmpty(content) == false)
                {
                    this.logger.LogInformation("Parsing completed with errors:");
                    this.logger.LogError("Error: {error}", content);
                    this.logger.LogInformation("");
                    this.logger.LogInformation("------------------------------");
                    this.logger.LogInformation("");

                    return null;
                }
                
            }

            this.logger.LogInformation("Parsing completed.");
            this.logger.LogInformation("");
            this.logger.LogInformation("------------------------------");
            this.logger.LogInformation("");

            var template = File.ReadAllText("ace-terraform-parsed-plan.json");
            return template;
        }
    }
}
