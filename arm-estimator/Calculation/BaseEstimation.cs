internal abstract class BaseEstimation
{
    internal static readonly int HoursInMonth = 720;
    internal readonly RetailItem[] items;
    internal readonly WhatIfAfterChange change;

    public BaseEstimation(RetailItem[] items, WhatIfAfterChange change)
    {
        this.items = items;
        this.change = change;
    }
}
