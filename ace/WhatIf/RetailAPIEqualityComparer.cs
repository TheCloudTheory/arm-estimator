using System.Diagnostics.CodeAnalysis;

namespace ACE.WhatIf;

/// <summary>
/// Used to filter items fetched from Retail API, as for some cases
/// it returns duplicate of data. It could be done via using only skuId, but 
/// as identifiers tends to differ for different regions, we cannot use it.
/// </summary>
internal class RetailAPIEqualityComparer : IEqualityComparer<RetailItem>
{
    public bool Equals(RetailItem? x, RetailItem? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;

        return x.skuName == y.skuName && x.productName == y.productName && x.meterName == y.meterName;
    }

    public int GetHashCode([DisallowNull] RetailItem obj)
    {
        return obj.skuName!.GetHashCode();
    }
}
