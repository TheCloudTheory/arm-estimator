internal class EstimateOptions
{
    public EstimateOptions(DeploymentMode mode,
                           int threshold,
                           FileInfo? parametersFile,
                           CurrencyCode currency,
                           bool shouldGenerateOutput,
                           bool shouldBeSilent,
                           bool stdout)
    {
        Mode = mode;
        Threshold = threshold;
        ParametersFile = parametersFile;
        Currency = currency;
        ShouldGenerateJsonOutput = shouldGenerateOutput;
        ShouldBeSilent = shouldBeSilent;
        Stdout = stdout;
    }

    public DeploymentMode Mode { get; }
    public int Threshold { get; }
    public FileInfo? ParametersFile { get; }
    public CurrencyCode Currency { get; }
    public bool ShouldGenerateJsonOutput { get; }
    public bool ShouldBeSilent { get; }
    public bool Stdout { get; }
}