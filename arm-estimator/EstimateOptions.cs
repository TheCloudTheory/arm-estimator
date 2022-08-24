internal class EstimateOptions
{
    public EstimateOptions(DeploymentMode mode,
                           int threshold,
                           FileInfo? parametersFile,
                           CurrencyCode currency,
                           bool shouldGenerateOutput,
                           bool shouldBeSilent)
    {
        Mode = mode;
        Threshold = threshold;
        ParametersFile = parametersFile;
        Currency = currency;
        ShouldGenerateOutput = shouldGenerateOutput;
        ShouldBeSilent = shouldBeSilent;
    }

    public DeploymentMode Mode { get; }
    public int Threshold { get; }
    public FileInfo? ParametersFile { get; }
    public CurrencyCode Currency { get; }
    public bool ShouldGenerateOutput { get; }
    public bool ShouldBeSilent { get; }
}