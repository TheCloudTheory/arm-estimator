using Azure.Core;
using Microsoft.Extensions.Logging;

namespace ACE.Output;

internal class HtmlOutputGenerator
{
    private readonly string fileName;
    private readonly EstimationOutput output;
    private readonly ILogger<Program> logger;

    public HtmlOutputGenerator(EstimationOutput output, ILogger<Program> logger, string? htmlOutputFilename)
    {
        this.fileName = string.IsNullOrEmpty(htmlOutputFilename) ?
            $"ace_estimation_{DateTime.UtcNow:yyyyMMddHHmmss}.html" :
            $"{htmlOutputFilename}.html";

        this.output = output;
        this.logger = logger;
    }

    public void Generate()
    {
        this.logger.AddEstimatorMessage("Generating HTML output file as {0}", this.fileName);

        var templatePath = Path.Combine("Html", "GeneratorTemplate.html");
        var template = File.ReadAllText(templatePath);
        const string tableRow = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";
        var resultRows = string.Empty;

        foreach (var resource in output.Resources)
        {
            var id = new ResourceIdentifier(resource.Id);
            resultRows += string.Format(tableRow, id.Name, id.ResourceType, $"{resource.TotalCost.Value:F} {output.Currency}", $"{resource.Delta.Value:F} {output.Currency}");
        }

        template = template.Replace("### [ACE-TBODY] ###", resultRows);
        template = template.Replace("### [ACE-TFOOT-TOTALSUM] ###", $"{output.TotalCost.Value:F} {output.Currency}");
        template = template.Replace("### [ACE-TFOOT-DELTASUM] ###", $"{output.Delta.Value:F} {output.Currency}");
        template = template.Replace("### [ACE-FOOTER-REPORTDATE] ###", DateTime.Now.ToString());
        template = template.Replace("### [ACE-BASIC-TD-ALLRESOURCES] ###", output.TotalResourceCount.ToString());
        template = template.Replace("### [ACE-BASIC-TD-ESTIMATEDRESOURCES] ###", output.EstimatedResourceCount.ToString());
        template = template.Replace("### [ACE-BASIC-TD-SKIPPEDRESOURCES] ###", output.SkippedResourceCount.ToString());

        File.WriteAllText(fileName, template);
    }
}