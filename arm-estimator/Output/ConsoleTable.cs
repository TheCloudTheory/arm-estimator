using Microsoft.Extensions.Logging;
using System.Text;

namespace ACE.Output
{
    /// <summary>
    /// Implementation of a table, which can be rendered as one of the possible output formats.
    /// </summary>
    internal class ConsoleTable
    {
        private readonly string title;
        private readonly string[] headers;
        private readonly ILogger logger;
        private readonly List<string[]> rows = new();
        private readonly Dictionary<int, int> maxColumnsWidth = new();

        public ConsoleTable(string title, string[] headers, ILogger logger)
        {
            this.title = title;
            this.headers = headers;
            this.logger = logger;
        }

        /// <summary>
        /// Adds a table row to render when Draw() method is called
        /// </summary>
        /// <param name="values">Values of a row represented as array of columns.</param>
        public void AddRow(string[] values)
        {
            rows.Add(values);
        }

        /// <summary>
        /// Draw a table based on provided headers and rows. 
        /// </summary>
        public void Draw()
        {
            CalculateColumnsWidth();

            DrawHeader();
            DrawRows();
        }

        /// <summary>
        /// As width of headers will most probably differ from width of rows,
        /// it's required to find max widths for each column and save them
        /// for table rendering. Here logic is quite simple - find width
        /// of each header and then find max width of a row column. If header
        /// is bigger than max value of a row column, use its width. Otherwise,
        /// use the found value.
        /// </summary>
        private void CalculateColumnsWidth()
        {
            for(var i = 0; i < this.headers.Length; i++)
            {
                var headerWidth = this.headers[i].Length;
                var maxColumnWidth = this.rows.Max(_ => _[i].Length);

                if(headerWidth > maxColumnWidth)
                {
                    this.maxColumnsWidth.Add(i, headerWidth);
                }
                else
                {
                    this.maxColumnsWidth.Add(i, maxColumnWidth);
                }
            }
        }

        /// <summary>
        /// Draw a table header containing both title of a table and
        /// header columns.
        /// </summary>
        private void DrawHeader()
        {
            DrawTitle();
            DrawBorder('┌', '┐', '┬');
            var output = new StringBuilder();

            for (var i = 0; i < this.headers.Length; i++)
            {
                var beggining = string.Empty;
                if(i == 0)
                {
                    beggining = "|";
                }

                output.Append($"{beggining}{this.headers[i]}{string.Join(string.Empty, Enumerable.Repeat(" ", CalculateWidthOfSpacer(this.headers[i].Length, this.maxColumnsWidth[i])))}|");
            }

            this.logger.LogInformation("{header}", output);
            DrawBorder('├', '┤', '┼');
        }

        /// <summary>
        /// Draw title of a table. Here's one gotcha - it may be impossible to divide 
        /// </summary>
        private void DrawTitle()
        {
            var totalTableWidth = this.maxColumnsWidth.Sum(_ => _.Value) + this.headers.Length - 1;
            var titleLength = this.title.Length;
            var spacerWidth = totalTableWidth - titleLength;
            var dividable = spacerWidth % 2;

            if(dividable == 0)
            {
                var width = spacerWidth / 2;
                var output = new StringBuilder();

                output.Append(string.Join(string.Empty, Enumerable.Repeat(" ", width)));
                output.Append(this.title);
                output.Append(string.Join(string.Empty, Enumerable.Repeat(" ", width)));

                this.logger.LogInformation("{header}", output);
            }
            else
            {
                var width = spacerWidth / 2;
                var output = new StringBuilder();

                output.Append(string.Join(string.Empty, Enumerable.Repeat(" ", width + 1)));
                output.Append(this.title);
                output.Append(string.Join(string.Empty, Enumerable.Repeat(" ", width)));

                this.logger.LogInformation("{header}", output);
            }
        }

        private void DrawBorder(char firstChar, char lastChar, char dividerChar)
        {
            var output = new StringBuilder();

            output.Append(firstChar);

            for (var i = 0; i < this.headers.Length; i++)
            {
                var divider = this.headers.Length - 1 == i ? null : (char?)dividerChar;
                output.Append($"{string.Join(string.Empty, Enumerable.Repeat('─', this.maxColumnsWidth[i]))}{divider}");
            }

            output.Append(lastChar);

            this.logger.LogInformation("{header}", output);
        }

        private void DrawRows()
        {
            var index = 1;
            var totalRows = this.rows.Count();

            foreach(var row in this.rows)
            {
                var output = new StringBuilder();
                for (var i = 0; i < row.Length; i++)
                {
                    var beggining = string.Empty;
                    if (i == 0)
                    {
                        beggining = "|";
                    }

                    output.Append($"{beggining}{row[i]}{string.Join(string.Empty, Enumerable.Repeat(" ", CalculateWidthOfSpacer(row[i].Length, this.maxColumnsWidth[i])))}|");
                }

                this.logger.LogInformation("{row}", output);

                if (index != totalRows)
                {
                    DrawBorder('├', '┤', '┼');
                }       
                else
                {
                    DrawBorder('└', '┴', '┘');
                }

                index++;
            }
        }

        private int CalculateWidthOfSpacer(int valueLength, int columnWidth)
        {
            return columnWidth - valueLength;
        }
    }
}
