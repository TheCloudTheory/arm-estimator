using ACE.Calculation;
using ACE.Extensions;
using ACE.WhatIf;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace ACE.Output
{
    internal class TableOutputFormatter : IOutputFormatter
    {
        private readonly ConsoleTable estimationsTable;
        private readonly Table freeResourcesTable;
        private readonly Table unsupportedResourcesTable;
        private readonly CurrencyCode currency;

        public TableOutputFormatter(CurrencyCode currency, ILogger logger)
        {
            this.estimationsTable = new ConsoleTable("Estimation", new[]
            {
                "Change type",
                "Resource name",
                "Resource type",
                "Location",
                "Total cost",
                "Delta"
            }, logger);

            this.freeResourcesTable = new Table
            {
                Title = new TableTitle("Free Resources")
            };

            this.unsupportedResourcesTable = new Table
            {
                Title = new TableTitle("Unsupported Resources")
            };

            this.currency = currency;
        }

        public void BeginEstimationsBlock()
        {
        }

        public void EndEstimationsBlock()
        {
            this.estimationsTable.Draw();
        }

        public void RenderFreeResourcesBlock(Dictionary<CommonResourceIdentifier, WhatIfChangeType?> freeResources)
        {
            this.freeResourcesTable.AddColumn("Change type");
            this.freeResourcesTable.AddColumn("Resource name");
            this.freeResourcesTable.AddColumn("Resource type");

            foreach(var resource in freeResources)
            {
                this.freeResourcesTable.AddRow(
                    resource.Value.ToString().GetValueOrNotAvailable(),
                    resource.Key.GetName(), 
                    resource.Key.GetResourceType().GetValueOrNotAvailable());
            }

            AnsiConsole.Write(this.freeResourcesTable);
        }

        public void RenderUnsupportedResourcesBlock(List<CommonResourceIdentifier> unsupportedResources)
        {
            this.unsupportedResourcesTable.AddColumn("Resource name");
            this.unsupportedResourcesTable.AddColumn("Resource type");

            foreach (var resource in unsupportedResources)
            {
                this.unsupportedResourcesTable.AddRow(
                    resource.GetName(),
                    resource.GetResourceType().GetValueOrNotAvailable());
            }

            AnsiConsole.Write(this.unsupportedResourcesTable);
        }

        public void ReportEstimationToConsole(CommonResourceIdentifier id,
                                              IOrderedEnumerable<RetailItem> items,
                                              TotalCostSummary summary,
                                              WhatIfChangeType? changeType,
                                              double? delta,
                                              string? location)
        {
            var deltaSign = delta == null ? "+" : delta == 0 ? "" : "-";
            delta = delta == null ? summary.TotalCost : 0;

            this.estimationsTable.AddRow(new[] {
                changeType.ToString().GetValueOrNotAvailable(),
                id.GetName(),
                id.GetResourceType().GetValueOrNotAvailable(),
                location.GetValueOrNotAvailable(),
                $"{summary.TotalCost}" +
                $" {this.currency}",
                $"{deltaSign}{delta} {this.currency}"});
        }
    }
}
