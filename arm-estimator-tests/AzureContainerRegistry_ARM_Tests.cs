namespace arm_estimator_tests
{
    public class AzureContainerRegistry_ARM_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var ace = await Program.Main(new[] { "templates/acr.json", "f81e70a7-e819-49b2-a980-8e9c433743dd", "arm-estimator-rg", "--generateJsonOutput" });
        }
    }
}