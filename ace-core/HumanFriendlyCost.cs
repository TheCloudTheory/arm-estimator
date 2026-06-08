using System.Globalization;
using System.Text.Json.Serialization;

namespace ACE
{
    /// <summary>A cost value with both a formatted string and the raw double.</summary>
    public record HumanFriendlyCost
    {
        private static readonly NumberFormatInfo Precision = new()
        {
            NumberDecimalDigits = 2
        };

        public HumanFriendlyCost(double value)
        {
            Value = value.ToString("N", Precision);
            OriginalValue = value;
        }

        [JsonConstructor]
        public HumanFriendlyCost(string value, double originalValue)
        {
            Value = value;
            OriginalValue = originalValue;
        }

        public string Value { get; private set; }
        public double OriginalValue { get; private set; }
    }
}
