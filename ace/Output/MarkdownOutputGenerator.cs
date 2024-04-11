using Azure.Core;
using Microsoft.Extensions.Logging;

namespace ACE.Output;

internal class MarkdownOutputGenerator
{
    private readonly string fileName;
    private readonly EstimationOutput output;
    private readonly ILogger<Program> logger;
    
    public MarkdownOutputGenerator(EstimationOutput output, ILogger<Program> logger, string? outputFilename)
    {
        this.fileName = string.IsNullOrEmpty(outputFilename) ?
            $"ace_estimation_{DateTime.UtcNow:yyyyMMddHHmmss}.md" :
            $"{outputFilename}.md";

        this.output = output;
        this.logger = logger;
    }

    public void Generate()
    {
        this.logger.AddEstimatorMessage("Generating Markdown output file as {0}", this.fileName);

        var templatePath = Path.Combine("Markdown", "GeneratorTemplate.md");
        var template = File.ReadAllText(templatePath);
        const string tableRow = "{0}|{1}|{2}|{3}";
        var resultRows = string.Empty;

        foreach (var resource in output.Resources)
        {
            var id = new ResourceIdentifier(resource.Id);
            resultRows += string.Format(tableRow, id.Name, id.ResourceType, $"{resource.TotalCost:F} {output.Currency}", $"{resource.Delta:F} {output.Currency}");
        }

        template = template.Replace("@@@ [ACE-TBODY] @@@", resultRows);
        template = template.Replace("@@@ [ACE-TFOOT-TOTALSUM] @@@", $"{output.TotalCost:F} {output.Currency}");
        template = template.Replace("@@@ [ACE-TFOOT-DELTASUM] @@@", $"{output.Delta:F} {output.Currency}");
        template = template.Replace("@@@ [ACE-FOOTER-REPORTDATE] @@@", DateTime.Now.ToString());
        template = template.Replace("@@@ [ACE-BASIC-TD-ALLRESOURCES] @@@", output.TotalResourceCount.ToString());
        template = template.Replace("@@@ [ACE-BASIC-TD-ESTIMATEDRESOURCES] @@@", output.EstimatedResourceCount.ToString());
        template = template.Replace("@@@ [ACE-BASIC-TD-SKIPPEDRESOURCES] @@@", output.SkippedResourceCount.ToString());

        File.WriteAllText(fileName, template);
    }
}
