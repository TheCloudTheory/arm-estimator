using ACE.WhatIf;
using Microsoft.Extensions.Logging;

namespace ACE.Output
{
    /// <summary>
    /// Defines used output formatter based on selected output format type
    /// </summary>
    internal class OutputGenerator
    {
        private readonly IOutputFormatter formatter;

        public OutputGenerator(
            OutputFormat format,
            ILogger logger,
            CurrencyCode currency,
            bool disableDetailedMetrics)
        {
            if(format == OutputFormat.Default)
            {
                this.formatter = new DefaultOutputFormatter(logger, currency, disableDetailedMetrics);
            }
            else
            {
                this.formatter = new TableOutputFormatter(currency);
            }
        }

        public IOutputFormatter GetFormatter()
        {
            return this.formatter;
        }
    }
}
