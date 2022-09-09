using Azure.Core;
using Microsoft.Extensions.Logging;

internal class HtmlOutputGenerator
{
    private readonly string fileName;
    private readonly EstimationOutput output;
    private readonly ILogger<Program> logger;

    public HtmlOutputGenerator(EstimationOutput output, ILogger<Program> logger)
    {
        this.fileName = $"ace_estimation_{DateTime.UtcNow:yyyyMMddHHmmss}.html";
        this.output = output;
        this.logger = logger;
    }

    public void Generate()
    {
        this.logger.AddEstimatorMessage("Generating HTML output file as {0}", fileName);

        var templatePath = Path.Combine("Html", "GeneratorTemplate.html");
        var template = File.ReadAllText(templatePath);
        const string tableRow = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";
        var resultRows = string.Empty;

        foreach(var resource in this.output.Resources)
        {
            var id = new ResourceIdentifier(resource.Id);
            resultRows += string.Format(tableRow, id.Name, id.ResourceType, resource.TotalCost, resource.Delta);
        }

        template = template.Replace("### [ACE-TBODY] ###", resultRows);
        File.WriteAllText(this.fileName, template);
    }
}
