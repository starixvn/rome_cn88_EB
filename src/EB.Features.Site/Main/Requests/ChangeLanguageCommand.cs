namespace EB.Features.Site.Main.Requests
{
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class ChangeLanguageCommand : IRequest<string>
    {
        private readonly UserSession userSession;

        public ChangeLanguageCommand(UserSession userSession, string languageCode)
        {
            this.userSession = userSession;
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

            userSession.Language = languageCode;
        }
    }
}
