using System;

namespace EB.Features.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        private const string ShortDate = "yyyy-MM-dd";

        public static string ToShortDate(this DateTime dateTime)
            => dateTime.ToString(ShortDate);
    }
}
