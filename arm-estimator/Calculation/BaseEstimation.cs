using Azure.Core;

internal abstract class BaseEstimation
{
    internal static readonly int HoursInMonth = 720;
    internal readonly RetailItem[] items;
    internal readonly ResourceIdentifier id;
    internal readonly WhatIfAfterBeforeChange change;

    public BaseEstimation(RetailItem[] items, ResourceIdentifier id, WhatIfAfterBeforeChange change)
    {
        this.items = items;
        this.id = id;
        this.change = change;
    }
}
