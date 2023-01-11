using Microsoft.Extensions.Logging;

namespace ACE.Compilation
{
    internal class TemplateCompiler
    {
        private readonly FileInfo templateFile;
        private readonly ILogger<Program> logger;
        private readonly ICompiler compiler;

        public TemplateCompiler(FileInfo templateFile, ILogger<Program> logger)
        {
            this.templateFile = templateFile;
            this.logger = logger;
            this.compiler = DetermineCompiler();
        }

        private ICompiler DetermineCompiler()
        {
            if(this.templateFile.Extension == ".bicep")
            {
                return new BicepCompiler(this.logger);
            }

            if(this.templateFile.Extension == ".tf")
            {
                return new TerraformCompiler();
            }

            return new ArmTemplateCompiler();
        }

        public string? Compile()
        {
            return this.compiler.Compile(this.templateFile);
        }
    }
}
