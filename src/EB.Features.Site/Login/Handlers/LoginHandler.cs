namespace EB.Features.Site.Login.Handlers
{
    using System;
    using EB.Features.Main.Resources;
    using EB.Features.Main.Services;
    using EB.Features.Site.Login.Requests;
    using EB.Features.Site.Shared.Models;
    using Fanex.Common;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;

    public class LoginHandler :
        IRequestHandler<LoginQuery, LoginViewModel>,
        IRequestHandler<SSOQuery, LoginViewModel>
    {
        private const int SSOExpiredTimeInSeconds = 30;

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

                AssignUserSession(userInfo, onUserId, message.SessionId, message.HideLanguage);
            }
            
            return new LoginViewModel(errorMessage, userInfo, message.HideLanguage);
        }

        public LoginViewModel Handle(SSOQuery message)
        {
            if (ValidateSSOTime(message.Time))
            {
                var userId = Convert.ToInt32(Decrypt(message.UserId));
                var onUserId = SSOLogin(userId, message);

                if (onUserId > 0)
                {
                    var user = userInfoService.GetUserInfo(userId);

                    if (user != null && service.CheckUserSitePermission(user))
                    {
                        AssignUserSession(user, onUserId, message.SessionId, message.Language);

                        return new LoginViewModel(string.Empty, user, message.Language);
                    }
                }
            }

            return new LoginViewModel(MainResource.You_have_no_authorization, new User(), message.Language);
        }

        private bool ValidateSSOTime(string time)
        {
            var currentTime = Convert.ToDateTime(securityService.GetTime());
            var ssoTime = Convert.ToDateTime(Decrypt(time));
            var expiration = currentTime - ssoTime;

            return expiration.TotalSeconds <= SSOExpiredTimeInSeconds;
        }

        private int SSOLogin(int userId, SSOQuery message)
        {
            var onlineUserId = Convert.ToInt32(Decrypt(message.OnUserId));
            var loginParam = new SSOLoginParameter
            {
                UserId = userId,
                SiteId = message.SiteId,
                CheckOnUserId = Convert.ToInt32(onlineUserId),
                OnUserId = 0,
                IP = napClientContext.IP,
                NetSessionId = message.SessionId,
                LoginDate = DateTime.Now,
                BrowserName = napClientContext.BrowserName,
                CountryCode = napClientContext.CountryCode,
                Domain = napClientContext.DomainName,
                Platform = napClientContext.Platform
            };

            return securityService.SSOLogin(loginParam);
        }

        private void AssignUserSession(User user, int onUserId, string sessionId, string language)
        {
            napClientSession.UserId = user.Id;
            napClientSession.Username = user.Username;

            userSession.UserId = user.Id;
            userSession.UserName = user.Username;
            userSession.OnUserId = onUserId;
            userSession.SessionId = sessionId;
            userSession.Language = language;
        }

        private static string Decrypt(string text)
            => Cryptography.CfsDeCrypt(text.Replace(" ", "+"));
    }
}
