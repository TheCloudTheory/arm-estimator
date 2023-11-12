using System.Globalization;
using System.Text.Json.Serialization;

namespace ACE
{
    internal record HumanFriendlyCost
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
