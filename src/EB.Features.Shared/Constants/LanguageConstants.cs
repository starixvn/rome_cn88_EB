namespace EB.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class LanguageConstants
    {
        public const string NewChineseSimplified = "zh-Hans";
        public const string ChineseTraditional = "zh-TW";
        public const string English = "en-US";

        public const string Default = NewChineseSimplified;

        public static readonly Dictionary<string, string> CodeKeys = new Dictionary<string, string>
        {
            [NewChineseSimplified] = "CN",
            [ChineseTraditional] = "CT",
            [English] = "E"
        };

        public static readonly Dictionary<string, string> CodeNames = new Dictionary<string, string>
        {
            [NewChineseSimplified] = "简体",
            [ChineseTraditional] = "繁體",
            [English] = "English"
        };

        public static string GetLanguageName(string languageCode)
            => !string.IsNullOrEmpty(languageCode) && CodeNames.ContainsKey(languageCode) ? CodeNames[languageCode] : CodeNames[Default];

        public static string Standardize(string languageCode)
            => IsValidLanguage(languageCode) ? languageCode : Default;

        public static bool IsValidLanguage(string languageCode)
            => !string.IsNullOrWhiteSpace(languageCode) && CodeKeys.ContainsKey(languageCode);
    }
}
