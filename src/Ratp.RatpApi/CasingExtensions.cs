namespace Ratp.RatpApi
{
    internal static class CasingExtensions
    {
        internal static string PascalCaseToCamelCase(this string theString)
        {
            return char.ToLowerInvariant(theString[0]) + theString.Substring(1);
        }
    }
}