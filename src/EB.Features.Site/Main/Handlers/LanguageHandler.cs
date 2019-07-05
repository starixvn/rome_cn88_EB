namespace EB.Features.Site.Main.Handlers
{
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Main.ViewModel;
    using EB.Features.Site.Shared.Models;
    using MediatR;
    using Requests;

    public class LanguageHandler :
        IRequestHandler<ChangeLanguageCommand, string>,
        IRequestHandler<AllLanguagesQuery, LanguageViewModel>
    {
        private readonly UserSession userSession;

        public LanguageHandler(UserSession userSession)
        {
            this.userSession = userSession;
        }

        public string Handle(ChangeLanguageCommand message)
        {
            userSession.Language = message.LanguageCode;

            return userSession.Language;
        }

        public LanguageViewModel Handle(AllLanguagesQuery message)
            => new LanguageViewModel(LanguageConstants.CodeNames, userSession.Language);
    }
}
