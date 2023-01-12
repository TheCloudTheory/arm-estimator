using ACE.Compilation;
using Microsoft.Extensions.Logging;

namespace ACE.WhatIf
{
    internal class WhatIfParser
    {
        private readonly TemplateType templateType;
        private readonly string subscriptionId;
        private readonly string resourceGroupName;
        private readonly string template;
        private readonly DeploymentMode mode;
        private readonly string parameters;
        private readonly ILogger<Program> logger;

        public WhatIfParser(
            TemplateType templateType,
            string subscriptionId,
            string resourceGroupName,
            string template,
            DeploymentMode mode,
            string parameters,
            ILogger<Program> logger)
        {
            this.templateType = templateType;
            this.subscriptionId = subscriptionId;
            this.resourceGroupName = resourceGroupName;
            this.template = template;
            this.mode = mode;
            this.parameters = parameters;
            this.logger = logger;
        }

        public async Task<WhatIfResponse?> GetWhatIfData()
        {
            if(this.templateType == TemplateType.ArmTemplateOrBicep)
            {
                var handler = new AzureWhatIfHandler(
                    this.subscriptionId, 
                    this.resourceGroupName, 
                    this.template, 
                    this.mode, 
                    this.parameters, 
                    this.logger);

                return await handler.GetResponseWithRetries();
            }

            if(this.templateType == TemplateType.Terraform)
            {
                var parser = new TerraformTemplateParser(this.template, this.logger);
                return parser.GetConfigurationAsWhatIfData();
            }

            return null;
        }
    }
}
