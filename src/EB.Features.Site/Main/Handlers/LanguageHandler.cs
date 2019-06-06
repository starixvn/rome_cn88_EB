namespace EB.Features.Site.Main.Handlers
{
    using EB.Features.Shared.Constants;
    using MediatR;
    using Requests;

    public class LanguageHandler : IRequestHandler<ChangeLanguageCommand, string>
    {
        public string Handle(ChangeLanguageCommand message)
        {
            var languageCode = Standardize(message.LanguageCode);

            message.ChangeLanguage(languageCode);

            return languageCode;
        }

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
