namespace ACE_Tests.Reworked;

public class ConfigurationTests
{
    [Test]
    [Parallelizable(ParallelScope.Self)]
    public void WhenConfigurationIsPassedViaConfigurationFileAndInline_ItShouldReturnError()
    {
        var exitCode = Program.Main([
            "templates/bicep/availability-set.bicep",
            "cf70b558-b930-45e4-9048-ebcefb926adf",
            "arm-estimator-tests-rg",
            "--configuration-file",
            "templates/configuration/configuration.json",
            "--generate-json-output"
        ]);

        Assert.That(exitCode, Is.EqualTo(1));
    }
    
    [Test]
    [Parallelizable(ParallelScope.Self)]
    public void WhenConfigurationContainsCurrency_ItShouldWork()
    {
        var exitCode = Program.Main([
            "templates/reworked/automation-account/automation-account.bicep",
            "cf70b558-b930-45e4-9048-ebcefb926adf",
            "arm-estimator-tests-rg",
            "--configuration-file",
            "templates/configuration/configuration-currency.json"
        ]);

        Assert.That(exitCode, Is.EqualTo(0));
    }
}