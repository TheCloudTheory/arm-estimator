using Azure.Core;

internal abstract class BaseEstimation
{
    internal static readonly int HoursInMonth = 730;
    internal readonly RetailItem[] items;
    internal readonly ResourceIdentifier id;
    internal readonly WhatIfAfterBeforeChange change;

    public BaseEstimation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
    {
        this.items = items;
        this.id = id;
        this.change = change;
    }

    protected int IncludeUsagePattern(string key, IDictionary<string, string>? usagePatterns)
    {
        if(usagePatterns != null && usagePatterns.TryGetValue(key, out string? value))
        {
            if(value != null)
            {
                return int.Parse(value);
            }
        }

        return 1;
    }
}
