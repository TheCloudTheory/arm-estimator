using ACE.Calculation;
using ACE.WhatIf;

namespace ACE.Output
{
    internal interface IOutputFormatter
    {
        void ReportEstimationToConsole(CommonResourceIdentifier id,
                                               IOrderedEnumerable<RetailItem> items,
                                               TotalCostSummary summary,
                                               WhatIfChangeType? changeType,
                                               double? delta,
                                               string? location);

        void BeginEstimationsBlock();
        void EndEstimationsBlock();
        void RenderFreeResourcesBlock(Dictionary<CommonResourceIdentifier, WhatIfChangeType?> freeResources);
        void RenderUnsupportedResourcesBlock(List<CommonResourceIdentifier> unsupportedResources);
        void RenderOtherResourcesBlock(Dictionary<CommonResourceIdentifier, WhatIfChangeType?> otherResources);
    }
}
