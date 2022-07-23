internal abstract class BaseEstimation
{
    internal static readonly int HoursInMonth = 720;
    internal readonly RetailItem[] items;

    public BaseEstimation(RetailItem[] items)
    {
        this.items = items;
    }
}
