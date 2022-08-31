using System.Text.Json.Serialization;

namespace arm_estimator_tests
{
    public class EstimationOutput
    {
        public double TotalCost { get; set; }
        public double Delta { get; set; }
        public IEnumerable<EstimatedResourceData> Resources { get; set; }
        public string Currency { get; set; }

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public EstimationOutput() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }

    public class EstimatedResourceData
    {
        public string Id { get; set; }
        public double TotalCost { get; set; }
        public double Delta { get; set; }

        [JsonConstructor]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public EstimatedResourceData() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
