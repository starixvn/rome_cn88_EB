namespace EB.Features.Site
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using EB.Features.Main.Services;
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Login;
    using EB.Features.Site.Login.Requests;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;

    [AllowAnonymous]
    public class LoginController : MultipleLanguageController
    {
        private readonly IMediator mediator;
        private readonly ICurrentSession currentSession;
        private readonly ICurrentContext currentContext;
        private readonly IUnderMainternanceService maintenanceService;
        private readonly ISecurityService securityService;


        public LoginController(
            IMediator mediator,
            ICurrentContext currentContext,
            ISecurityService securityService,
            IUnderMainternanceService maintenanceService,
            ICurrentSession currentSession)
        {
            this.mediator = mediator;
            this.currentSession = currentSession;
            this.currentContext = currentContext;
            this.securityService = securityService;
            this.maintenanceService = maintenanceService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            if (!NapSettings.Settings.IsNapHub && !NapSettings.Settings.EnableDirectLogin)
            {
                // if login-directly feature is disabled then redirect users to NAP Hub
                return Redirect(NapSettings.Settings.HubUrl);
            }

            SetUMToken();

            return View("~/Login/Views/Index.cshtml", new LoginViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(LoginQuery query)
        {
            var viewModel = mediator.Send(query).ConfigureAwait(false).GetAwaiter().GetResult();

            if (string.IsNullOrEmpty(viewModel.ErrorMessage))
            {
                CheckIn(viewModel.UserInfo);
                CreateAuthInfo(viewModel.UserInfo);
                AssignSettingLanguage(query.HideLanguage);

                var redirectUrl = string.IsNullOrEmpty(query.ReturnUrl) ||
                                  query.ReturnUrl.IndexOf("Logout", StringComparison.CurrentCultureIgnoreCase) >=
                                  0
                    ? FormsAuthentication.DefaultUrl
                    : query.ReturnUrl;
                return Redirect(redirectUrl);
            }

            return View("~/Login/Views/Index.cshtml", viewModel);
        }

        public ActionResult LogOut()
        {
            DoSignOut();

            return Redirect(FormsAuthentication.LoginUrl);
        }

        [HttpPost]
        public ActionResult ReCheckin()
        {
            var isKickedOut = NapSettings.Settings.EnableReCheckIn
                && securityService.ReCheckIn(currentSession.OnUserId, NapSettings.Settings.SiteId) > 0;

            return Json(isKickedOut);
        }

        public ActionResult Offline()
        {
            var downTime = maintenanceService.UnderMainternanceInfo();

            if (!downTime.IsUM)
            {
                SetUMToken();

                FormsAuthentication.RedirectToLoginPage();
            }

            return View("~/Login/Views/UnderMaintenance.cshtml", downTime);
        }


        private void SetUMToken()
        {
            if (Request.QueryString["UMToken"] != null)
            {
                Session["UMToken"] = Request.QueryString["UMToken"];
            }
        }

        private void DoSignOut()
        {
            if (currentSession != null && currentSession.OnUserId != 0)
            {
                securityService.Logout(currentSession.OnUserId);
            }

            Session.Clear();
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();
        }

        private void CreateAuthInfo(User user)
        {
            var authInfo = new AuthInfo
            {
                UserId = user.Id,
                OnUserId = currentSession.OnUserId
            };

            var authTicket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.Now,
                DateTime.Now + FormsAuthentication.Timeout,
                false,
                authInfo.ToXml(),
                "/");

            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = "/",
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }
       
        private static void AssignSettingLanguage(string language)
        {
            var languageCode = LanguageConstants.Standardize(language);

            UserSessionImpl.Current.Language = languageCode;
        }

        private void CheckIn(User user)
        {
            // check-in user
            var loginParameter = new LoginParameter
            {
                UserId = user.Id,
                SiteId = NapSettings.Settings.SiteId,
                NetSessionId = Session.SessionID,
                Ip = currentContext.IP,
                LoginDate = DateTime.Now,
                BrowserName = currentContext.BrowserName,
                CountryCode = currentContext.CountryCode,
                Domain = currentContext.DomainName,
                Platform = currentContext.Platform
            };

            var onUserId = securityService.Login(loginParameter);

            // saves user session
            currentSession.UserId = user.Id;
            currentSession.Username = user.Username;
            currentSession.OnUserId = onUserId;
            currentSession.SessionId = Session.SessionID;
        }
    }
}
