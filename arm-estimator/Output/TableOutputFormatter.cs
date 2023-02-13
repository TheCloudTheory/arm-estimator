using ACE.Calculation;
using ACE.Extensions;
using ACE.WhatIf;
using Spectre.Console;

namespace ACE.Output
{
    internal class TableOutputFormatter : IOutputFormatter
    {
        private readonly Table estimationsTable;
        private readonly Table freeResourcesTable;
        private readonly Table unsupportedResourcesTable;
        private readonly CurrencyCode currency;

        public TableOutputFormatter(CurrencyCode currency)
        {
            this.estimationsTable = new Table
            {
                Title = new TableTitle("Estimations")
            };

            this.freeResourcesTable = new Table
            {
                Title = new TableTitle("Free Resources")
            };

            this.unsupportedResourcesTable = new Table
            {
                Title = new TableTitle("Unsupported Resources")
            };

            estimationsTable.Expand();
            freeResourcesTable.Expand();
            unsupportedResourcesTable.Expand();

            this.currency = currency;
        }

        public void BeginEstimationsBlock()
        {
            this.estimationsTable.AddColumn("Change type");
            this.estimationsTable.AddColumn("Resource name");
            this.estimationsTable.AddColumn("Resource type");
            this.estimationsTable.AddColumn("Location");
            this.estimationsTable.AddColumn("Total cost");
            this.estimationsTable.AddColumn("Delta");

            estimationsTable.Columns[0].NoWrap();
            estimationsTable.Columns[1].NoWrap();
            estimationsTable.Columns[2].NoWrap();
            estimationsTable.Columns[3].NoWrap();
            estimationsTable.Columns[4].NoWrap();
            estimationsTable.Columns[5].NoWrap();
        }

        public void EndEstimationsBlock()
        {
            AnsiConsole.Write(this.estimationsTable);
        }

        public void RenderFreeResourcesBlock(Dictionary<CommonResourceIdentifier, WhatIfChangeType?> freeResources)
        {
            this.freeResourcesTable.AddColumn("Change type");
            this.freeResourcesTable.AddColumn("Resource name");
            this.freeResourcesTable.AddColumn("Resource type");

            this.freeResourcesTable.Columns[0].NoWrap();
            this.freeResourcesTable.Columns[1].NoWrap();
            this.freeResourcesTable.Columns[2].NoWrap();

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

            this.unsupportedResourcesTable.Columns[0].NoWrap();
            this.unsupportedResourcesTable.Columns[1].NoWrap();

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

            this.estimationsTable.AddRow(
                changeType.ToString().GetValueOrNotAvailable(), 
                id.GetName(), 
                id.GetResourceType().GetValueOrNotAvailable(), 
                location.GetValueOrNotAvailable(), 
                $"{summary.TotalCost}" +
                $" {this.currency}", 
                $"{deltaSign}{delta} {this.currency}");
        }
    }
}
