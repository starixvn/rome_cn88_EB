namespace EB.Features.Site.Main.ViewModel
{
    using System.Collections.Generic;
    using EB.Features.Shared.Constants;

    public class LanguageViewModel
    {
        public LanguageViewModel(
              Dictionary<string, string> data,
              string currentLanguageCode = Language.Default)
        {
            Data = data ?? Language.CodeNames;
            CurrentLanguageCode = currentLanguageCode;

            CurrentLanguageName = Language.CodeNames[CurrentLanguageCode];
        }

        public Dictionary<string, string> Data { get; }

        public string CurrentLanguageCode { get; }

        public string CurrentLanguageName { get; }
    }
}
