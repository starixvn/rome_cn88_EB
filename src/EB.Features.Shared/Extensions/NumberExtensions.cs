namespace EB.Features.Shared.Extensions
{
    public static class NumberExtensions
    {
        public static string ToNumberWithoutDecimal(this int value)
            => value.ToString("#,##0");

        public static string ToNumberWithoutDecimal(this decimal value)
            => value.ToString("#,##0");

        public static bool InRange(this int number, int start, int end)
            => start <= number && number <= end;

        public static decimal Round(this decimal value)
            => decimal.Round(value, 2);
    }
}
