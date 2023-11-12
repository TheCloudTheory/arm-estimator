using ACE.Calculation;
using ACE.Extensions;
using ACE.WhatIf;
using Microsoft.Extensions.Logging;

namespace ACE.Output
{
    internal class TableOutputFormatter : IOutputFormatter
    {
        private readonly ConsoleTable estimationsTable;
        private readonly ConsoleTable freeResourcesTable;
        private readonly ConsoleTable unsupportedResourcesTable;
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

            this.freeResourcesTable = new ConsoleTable("Free Resources", new[]
            {
                "Change type",
                "Resource name",
                "Resource type"
            }, logger);

            this.unsupportedResourcesTable = new ConsoleTable("Unsupported Resources", new[]
            {
                "Resource name",
                "Resource type"
            }, logger);

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
            foreach(var resource in freeResources)
            {
                this.freeResourcesTable.AddRow(new[] {
                    resource.Value.ToString().GetValueOrNotAvailable(),
                    resource.Key.GetName(),
                    resource.Key.GetResourceType().GetValueOrNotAvailable()});
            }

            this.freeResourcesTable.Draw();
        }

        public void RenderUnsupportedResourcesBlock(List<CommonResourceIdentifier> unsupportedResources)
        {
            foreach (var resource in unsupportedResources)
            {
                this.unsupportedResourcesTable.AddRow(new[] {
                    resource.GetName(),
                    resource.GetResourceType().GetValueOrNotAvailable() });
            }

            this.unsupportedResourcesTable.Draw();
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
