internal class EstimateOptions
{
    public EstimateOptions(FileInfo templateFile,
                           string subscriptionId,
                           string resourceGroupName,
                           DeploymentMode mode,
                           int threshold,
                           FileInfo? parametersFile,
                           CurrencyCode currency,
                           bool shouldGenerateOutput)
    {
        TemplateFile = templateFile;
        SubscriptionId = subscriptionId;
        ResourceGroupName = resourceGroupName;
        Mode = mode;
        Threshold = threshold;
        ParametersFile = parametersFile;
        Currency = currency;
        ShouldGenerateOutput = shouldGenerateOutput;
    }

    public FileInfo TemplateFile { get; }
    public string SubscriptionId { get; }
    public string ResourceGroupName { get; }
    public DeploymentMode Mode { get; }
    public int Threshold { get; }
    public FileInfo? ParametersFile { get; }
    public CurrencyCode Currency { get; }
    public bool ShouldGenerateOutput { get; }
}