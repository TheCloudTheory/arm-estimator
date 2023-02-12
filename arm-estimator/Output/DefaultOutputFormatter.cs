using ACE.Calculation;
using ACE.WhatIf;
using Microsoft.Extensions.Logging;

namespace ACE.Output
{
    internal class DefaultOutputFormatter : IOutputFormatter
    {
        private readonly ILogger logger;
        private readonly CurrencyCode currency;
        private readonly bool disableDetailedMetrics;

        public DefaultOutputFormatter(
            ILogger logger,
            CurrencyCode currency,
            bool disableDetailedMetrics)
        {
            this.logger = logger;
            this.currency = currency;
            this.disableDetailedMetrics = disableDetailedMetrics;
        }

        public void BeginEstimationsBlock()
        {
            this.logger.LogInformation("Estimations:");
            this.logger.LogInformation("");
        }

        public void EndEstimationsBlock()
        {
        }

        public void RenderFreeResourcesBlock(Dictionary<CommonResourceIdentifier, WhatIfChangeType?> freeResources)
        {
            this.logger.LogInformation("Free resources:");
            this.logger.LogInformation("");

            foreach (var resource in freeResources)
            {
                ReportResourceWithoutCost(resource.Key, resource.Value);
            }

            this.logger.LogInformation("");
            this.logger.LogInformation("-------------------------------");
            this.logger.LogInformation("");
        }

        private EstimatedResourceData ReportResourceWithoutCost(CommonResourceIdentifier id, WhatIfChangeType? changeType)
        {
            logger.AddEstimatorMessageSensibleToChange(changeType, "{0}", id.GetName());
            logger.AddEstimatorMessageSubsection("Type: {0}", id.GetResourceType());
            logger.LogInformation("");

            return new EstimatedResourceData(0, 0, id);
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

            this.logger.AddEstimatorMessageSensibleToChange(changeType, "{0}", id.GetName());
            this.logger.AddEstimatorMessageSubsection("Type: {0}", id.GetResourceType());
            this.logger.AddEstimatorMessageSubsection("Location: {0}", location);
            this.logger.AddEstimatorMessageSubsection("Total cost: {0} {1}", summary.TotalCost.ToString("N2"), currency);
            this.logger.AddEstimatorMessageSubsection("Delta: {0} {1}", $"{deltaSign}{delta.GetValueOrDefault().ToString("N2")}", currency);

            if (this.disableDetailedMetrics == false)
            {
                ReportAggregatedMetrics(summary);
                ReportUsedMetrics(items);
            }

            this.logger.LogInformation("");
            this.logger.LogInformation("-------------------------------");
            this.logger.LogInformation("");
        }

        private void ReportAggregatedMetrics(TotalCostSummary summary)
        {
            this.logger.LogInformation("");
            this.logger.LogInformation("Aggregated metrics:");
            this.logger.LogInformation("");

            if (summary.DetailedCost.Count == 0)
            {
                this.logger.LogInformation("No metrics available.");
                return;
            }

            foreach (var metric in summary.DetailedCost)
            {
                this.logger.LogInformation("-> {metricName} [{cost} {currency}]", metric.Key, metric.Value, currency);
            }
        }

        private void ReportUsedMetrics(IOrderedEnumerable<RetailItem> items)
        {
            this.logger.LogInformation("");
            this.logger.LogInformation("Used metrics:");
            this.logger.LogInformation("");

            if (items.Any())
            {
                foreach (var item in items)
                {
                    this.logger.LogInformation("-> {skuName} | {productName} | {meterName} | {retailPrice} for {measure}", item.skuName, item.productName, item.meterName, item.retailPrice, item.unitOfMeasure);
                }
            }
            else
            {
                this.logger.LogInformation("No metrics available.");
            }
        }
    }
}
