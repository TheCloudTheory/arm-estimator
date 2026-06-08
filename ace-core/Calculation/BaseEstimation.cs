using ACE.WhatIf;

namespace ACE.Calculation;

internal abstract class BaseEstimation
{
    protected static readonly int HoursInMonth = 730;
    protected readonly RetailItem[] items;
    protected readonly CommonResourceIdentifier id;
    protected readonly WhatIfAfterBeforeChange change;
    protected readonly double conversionRate;

    public BaseEstimation(
        RetailItem[] items, 
        CommonResourceIdentifier id, 
        WhatIfAfterBeforeChange change,
        double conversionRate)
    {
        this.items = items;
        this.id = id;
        this.change = change;
        this.conversionRate = conversionRate;
    }

    protected int IncludeUsagePattern(string key, IDictionary<string, string>? usagePatterns, int defaultUsage = 1)
    {
        if (usagePatterns != null && usagePatterns.TryGetValue(key, out string? value))
        {
            if (value != null)
            {
                return int.Parse(value);
            }
        }

        return defaultUsage;
    }
}