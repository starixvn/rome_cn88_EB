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
        private readonly IUserInfoService userInfoService;

        public LoginHandler(IAuthService service, IUserInfoService userInfoService)
        {
            this.service = service;
            this.userInfoService = userInfoService;
        }

        public LoginViewModel Handle(LoginQuery message)
        {
            var loginMessage = service.Login(message.UserName, message.Password);
            var userInfo = userInfoService.GetUserInfo(message.UserName);
            var languageCode = LanguageConstants.Standardize(message.HideLanguage);

            UserSessionImpl.Current.Language = languageCode;

            return new LoginViewModel(loginMessage, userInfo);
        }

    }
}
