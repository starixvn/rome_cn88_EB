namespace EB.Features.Site.Login.Handlers
{
    using System.Globalization;
    using System.Threading;
    using EB.Features.Main.Services;
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Login.Requests;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class LoginHandler : IRequestHandler<LoginQuery, LoginViewModel>
    {
        private readonly IAuthService service;

        public LoginHandler(IAuthService service)
        {
            this.service = service;
        }

        public LoginViewModel Handle(LoginQuery message)
        {
            var loginMessage= service.Login(message.UserName, message.Password);
            var userInfo = service.GetUserInfo(message.UserName);
            var languageCode = LanguageConstants.Standardize(message.HideLanguage);

            UserSessionImpl.Current.Language = languageCode;

            return new LoginViewModel(loginMessage, userInfo);
        }
            
    }
}
