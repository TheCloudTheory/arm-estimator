namespace ACE.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Returns "N/A" if tested value is null. Used mostly in places where nullability check
        /// is not necessary (like output), while returning null value might be misleading or 
        /// difficult to interpret.
        /// </summary>
        public static string GetValueOrNotAvailable(this string? nullableValue)
        {
            return nullableValue ?? "N/A";
        }
    }
}
