namespace EB.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class Language
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
    }
}
