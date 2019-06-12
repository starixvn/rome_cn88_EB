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
            var languageCode = Language.Standardize(message.LanguageCode);

            message.ChangeLanguage(languageCode);

            return languageCode;
        }

        public LanguageViewModel Handle(AllLanguagesQuery message)
            => new LanguageViewModel(Language.CodeNames, Language.Standardize(message.LanguageCode));
    }
}
