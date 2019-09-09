namespace EB.Features.Site.Login.Handlers
{
    using System;
    using System.Web;
    using EB.Features.Main.Services;
    using EB.Features.Site.Login.Requests;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;

    public class LoginHandler : IRequestHandler<LoginQuery, LoginViewModel>
    {
        private readonly IAuthService service;
        private readonly IUserInfoService userInfoService;
        private readonly ICurrentContext napClientContext;
        private readonly ISecurityService securityService;
        private readonly UserSession userSession;
        private readonly ICurrentSession napClientSession;

        public LoginHandler(
            IAuthService service,
            IUserInfoService userInfoService,
            ISecurityService securityService,
            ICurrentContext currentContext,
            ICurrentSession napClientSession,
            UserSession userSession)
        {
            this.service = service;
            this.userInfoService = userInfoService;
            this.napClientContext = currentContext;
            this.securityService = securityService;
            this.userSession = userSession;
            this.napClientSession = napClientSession;
        }

        public LoginViewModel Handle(LoginQuery message)
        {
            var errorMessage = service.ValidateLogin(message.UserName, message.Password, userSession.Ip);
            var userInfo = new User();
            if(string.IsNullOrEmpty(errorMessage))
            {
                userInfo = userInfoService.GetUserInfo(message.UserName);
                var loginParameter = new LoginParameter
                {
                    UserId = userInfo.Id,
                    SiteId = message.SiteId,
                    NetSessionId = message.SessionId,
                    Ip = napClientContext.IP,
                    LoginDate = DateTime.Now,
                    BrowserName = napClientContext.BrowserName,
                    CountryCode = napClientContext.CountryCode,
                    Domain = napClientContext.DomainName,
                    Platform = napClientContext.Platform
                };

                var onUserId = securityService.Login(loginParameter);

                userSession.UserId = userInfo.Id;
                userSession.UserName = userInfo.Username;
                userSession.OnUserId = onUserId;
                userSession.SessionId = loginParameter.NetSessionId;
                userSession.Language = message.HideLanguage;

                napClientSession.Username = userInfo.Username;
                napClientSession.UserId = userInfo.Id;
            }
            
            return new LoginViewModel(errorMessage, userInfo, message.HideLanguage);
        }

    }
}
