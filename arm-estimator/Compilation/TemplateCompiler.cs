using Microsoft.Extensions.Logging;

namespace ACE.Compilation
{
    internal class TemplateCompiler
    {
        private readonly FileInfo templateFile;
        private readonly string? terraformExecutable;
        private readonly ILogger<Program> logger;
        private readonly ICompiler compiler;

        public TemplateType TemplateType { get; private set; }

        public TemplateCompiler(FileInfo templateFile, string? terraformExecutable, ILogger<Program> logger)
        {
            this.templateFile = templateFile;
            this.terraformExecutable = terraformExecutable;
            this.logger = logger;
            this.compiler = DetermineCompiler();
        }

        private ICompiler DetermineCompiler()
        {
            if(this.templateFile.Extension == ".bicep")
            {
                TemplateType = TemplateType.ArmTemplateOrBicep;
                return new BicepCompiler(this.logger);
            }

            if(this.templateFile.Extension == ".tf")
            {
                TemplateType = TemplateType.Terraform;
                return new TerraformCompiler(this.terraformExecutable, this.logger);
            }

            TemplateType = TemplateType.ArmTemplateOrBicep;
            return new ArmTemplateCompiler();
        }

        public string? Compile(CancellationToken token)
        {
            return this.compiler.Compile(this.templateFile, token);
        }
    }
}
