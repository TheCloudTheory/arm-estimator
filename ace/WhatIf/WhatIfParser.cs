using ACE.Compilation;
using Microsoft.Extensions.Logging;

namespace ACE.WhatIf;

internal class WhatIfParser
{
    private readonly TemplateType templateType;
    private readonly string scopeId;
    private readonly string? resourceGroupName;
    private readonly string template;
    private readonly ParametersSchema parameters;
    private readonly ILogger<Program> logger;
    private readonly CommandType commandType;
    private readonly string? location;
    private readonly EstimateOptions options;

    public WhatIfParser(
        TemplateType templateType,
        string scopeId,
        string? resourceGroupName,
        string template,
        ParametersSchema? parameters,
        ILogger<Program> logger,
        CommandType commandType,
        string? location,
        EstimateOptions options)
    {
        this.templateType = templateType;
        this.scopeId = scopeId;
        this.resourceGroupName = resourceGroupName;
        this.template = template;
        this.parameters = parameters ?? new ParametersSchema();
        this.logger = logger;
        this.commandType = commandType;
        this.location = location;
        this.options = options;
    }

    public async Task<WhatIfResponse?> GetWhatIfData(CancellationToken token)
    {
        if(this.templateType == TemplateType.ArmTemplateOrBicep)
        {
            var handler = new AzureWhatIfHandler(
                this.scopeId, 
                this.resourceGroupName, 
                this.template, 
                this.parameters, 
                this.logger,
                this.commandType,
                this.location,
                this.options);

            return await handler.GetResponseWithRetries(token);
        }

        if(this.templateType == TemplateType.Terraform)
        {
            var parser = new TerraformTemplateParser(this.template, this.logger);
            return parser.GetConfigurationAsWhatIfData(token);
        }

        return null;
    }
}
