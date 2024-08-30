namespace ACE_Tests.Reworked;

[Parallelizable(ParallelScope.Self)]
public class ErrorTests
{
    [Test]
    public void Error_WhenThereIsExceptionThrownBecauseOfInvalidTemplatePath_ACEShouldNotCrash()
    {
        var exitCode = Program.Main([
                "templates/reworked/key-vault/usage-pattern-1.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--inline",
                "parLocation=northeurope"
            ]);

        Assert.That(exitCode, Is.EqualTo(1));
    }

    [Test]
    public void Error_WhenThereIsExceptionThrownBecauseOfWrongParameter_ACEShouldNotCrash()
    {
        var exitCode = Program.Main([
                "templates/reworked/key-vault/usage-patterns-1.bicep",
                "cf70b558-b930-45e4-9048-ebcefb926adf",
                "arm-estimator-tests-rg",
                "--inline",
                "parLocation2=northeurope"
            ]);

        Assert.That(exitCode, Is.EqualTo(1));
    }
}