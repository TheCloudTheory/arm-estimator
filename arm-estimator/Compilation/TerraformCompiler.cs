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
            var workingDirectory = templateFile.Directory.FullName;
            var planFile = $"{templateFile.Directory}{Path.DirectorySeparatorChar}tfplan";

            this.logger.LogInformation("Starting parsing using embedded Go parser.");

            TerraformCompiler.GenerateParsedPlan(
                new GoString(workingDirectory, workingDirectory.Length),
                new GoString(planFile, planFile.Length));

            this.logger.LogInformation("Parsing completed.");
            this.logger.LogInformation("");
            this.logger.LogInformation("------------------------------");
            this.logger.LogInformation("");

            var template = File.ReadAllText("ace-terraform-parsed-plan.json");
            return template;
        }
    }
}
