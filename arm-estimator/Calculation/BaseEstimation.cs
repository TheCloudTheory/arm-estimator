internal abstract class BaseEstimation
{
    internal static readonly int HoursInMonth = 720;
    internal readonly RetailItem[] items;
    internal readonly WhatIfAfterBeforeChange change;

    public BaseEstimation(RetailItem[] items, WhatIfAfterBeforeChange change)
    {
        this.items = items;
        this.change = change;
    }
}
