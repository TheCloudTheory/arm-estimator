using System.CommandLine;
using System.CommandLine.Binding;

internal class EstimateOptionsBinder : BinderBase<EstimateOptions>
{
    private readonly Option<DeploymentMode> mode;
    private readonly Option<int> threshold;
    private readonly Option<FileInfo?> parameters;
    private readonly Option<CurrencyCode> currency;
    private readonly Option<bool> generateJsonOutput;
    private readonly Option<bool> shouldBeSilent;
    private readonly Option<bool> stdout;

    public EstimateOptionsBinder(Option<DeploymentMode> mode,
                                 Option<int> threshold,
                                 Option<FileInfo?> parameters,
                                 Option<CurrencyCode> currency,
                                 Option<bool> generateJsonOutput,
                                 Option<bool> shouldBeSilent,
                                 Option<bool> stdout)
    {
        this.mode = mode;
        this.threshold = threshold;
        this.parameters = parameters;
        this.currency = currency;
        this.generateJsonOutput = generateJsonOutput;
        this.shouldBeSilent = shouldBeSilent;
        this.stdout = stdout;
    }

    protected override EstimateOptions GetBoundValue(BindingContext bindingContext)
    {
        return new EstimateOptions(
            bindingContext.ParseResult.GetValueForOption(mode),
            bindingContext.ParseResult.GetValueForOption(threshold),
            bindingContext.ParseResult.GetValueForOption(parameters),
            bindingContext.ParseResult.GetValueForOption(currency),
            bindingContext.ParseResult.GetValueForOption(generateJsonOutput),
            bindingContext.ParseResult.GetValueForOption(shouldBeSilent),
            bindingContext.ParseResult.GetValueForOption(stdout)
            );
    }
}