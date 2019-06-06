namespace EB.Features.Site.Main.Requests
{
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class ChangeLanguageCommand : IRequest<string>
    {
        public ChangeLanguageCommand(string languageCode)
        {
            LanguageCode = languageCode;
        }

        public string LanguageCode { get; }

        public virtual void ChangeLanguage(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode) ||
                !Language.CodeKeys.ContainsKey(languageCode))
            {
                return;
            }

            UserSessionImpl.Current.Language = languageCode;
        }
    }
}
