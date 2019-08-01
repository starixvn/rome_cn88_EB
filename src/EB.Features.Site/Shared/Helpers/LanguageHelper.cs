namespace EB.Features.Site.Shared.Helpers
{
    using System.Collections.Generic;

    public static class LanguageHelper
    {
        private static readonly IDictionary<string, string> ValidLanguageKeys
            = new Dictionary<string, string>
            {
                ["en-US"] = "E",
                ["zh-Hans"] = "CN",
                ["zh-TW"] = "CT"
            };

        public static string GetLanguageKey(string language)
            => ValidLanguageKeys.TryGetValue(language ?? "zh-Hans", out var languageKey)
                ? languageKey
                : "CN";
    }
}