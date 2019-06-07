namespace EB.Features.Site.Main.Handlers
{
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Main.ViewModel;
    using MediatR;
    using Requests;

    public class LanguageHandler :
        IRequestHandler<ChangeLanguageCommand, string>,
        IRequestHandler<AllLanguagesQuery, LanguageViewModel>
    {
        public string Handle(ChangeLanguageCommand message)
        {
            var languageCode = Standardize(message.LanguageCode);

            message.ChangeLanguage(languageCode);

            return languageCode;
        }

        public LanguageViewModel Handle(AllLanguagesQuery message)
            => new LanguageViewModel(Language.CodeNames, Standardize(message.LanguageCode));

        private string Standardize(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode)
              || !Language.CodeKeys.ContainsKey(languageCode))
            {
                languageCode = Language.Default;
            }

            return languageCode;
        }
    }
}
