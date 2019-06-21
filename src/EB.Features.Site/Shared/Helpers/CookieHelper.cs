using System;

namespace EB.Features.Site.Shared.Helpers
{
    public static class CookieHelper
    {
        public static string GetCookieNameOfCurrentPage(
              int custId,
              int subAccId,
              Func<string, string> encodeDataFunction)
        {
            var joinStrings = new[]
            {
                custId.ToString(),
                subAccId.ToString(),
                "CN88"
            };

            var cacheUrlCookieName = string.Join("_", joinStrings);

            return encodeDataFunction?.Invoke(cacheUrlCookieName) ?? cacheUrlCookieName;
        }
    }
}