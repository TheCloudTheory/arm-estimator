using ACE.Calculation;
using ACE.WhatIf;

namespace ACE.Output;

/// <summary>
/// No-op implementation of <see cref="IOutputFormatter"/>. Used by
/// <see cref="ACE.Core.EstimationService"/> so that cost computation produces
/// no console or file side-effects.
/// </summary>
internal sealed class NullOutputFormatter : IOutputFormatter
{
    public void BeginEstimationsBlock() { }
    public void EndEstimationsBlock() { }

    public void ReportEstimationToConsole(
        CommonResourceIdentifier id,
        IOrderedEnumerable<RetailItem> items,
        TotalCostSummary summary,
        WhatIfChangeType? changeType,
        double? delta,
        string? location) { }

    public void RenderFreeResourcesBlock(
        Dictionary<CommonResourceIdentifier, WhatIfChangeType?> freeResources) { }

    public void RenderUnsupportedResourcesBlock(
        List<CommonResourceIdentifier> unsupportedResources) { }

    public void RenderOtherResourcesBlock(
        Dictionary<CommonResourceIdentifier, WhatIfChangeType?> otherResources) { }
}
