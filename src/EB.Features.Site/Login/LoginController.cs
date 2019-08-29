namespace EB.Features.Site
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using EB.Features.Main.Services;
    using EB.Features.Site.Login;
    using EB.Features.Site.Login.Requests;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Helpers;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using MediatR;

    [AllowAnonymous]
    public class LoginController : MultipleLanguageController
    {
        private readonly IMediator mediator;
        private readonly IUnderMainternanceService maintenanceService;
        private readonly ISecurityService securityService;
        private readonly UserSession userSession;

        public LoginController(
            IMediator mediator,
            ISecurityService securityService,
            IUnderMainternanceService maintenanceService,
            UserSession userSession)
        {
            this.mediator = mediator;
            this.securityService = securityService;
            this.maintenanceService = maintenanceService;
            this.userSession = userSession;
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

            return View("~/Login/Views/Index.cshtml", new LoginViewModel(userSession.Language));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(LoginQuery query)
        {
            query.SessionId = HttpContext.Session.SessionID;
            query.SiteId = NapSettings.Settings.SiteId;

            var viewModel = mediator.Send(query).ConfigureAwait(false).GetAwaiter().GetResult();

            if (string.IsNullOrEmpty(viewModel.ErrorMessage))
            {
                AuthInfoHelper.CreateAuthInfo(viewModel.UserInfo, userSession.OnUserId, HttpContext);

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

        [NapSuperAuth]
        [HttpPost]
        public ActionResult ReCheckin()
        {
            var isKickedOut = NapSettings.Settings.EnableReCheckIn
                && securityService.ReCheckIn(userSession.OnUserId, NapSettings.Settings.SiteId) > 0;

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

            if (Request.QueryString["AlphaUMToken"] != null)
            {
                Session["AlphaUMToken"] = Request.QueryString["AlphaUMToken"];
            }
        }

        private void DoSignOut()
        {
            if (userSession.OnUserId != 0)
            {
                securityService.Logout(userSession.OnUserId);
            }

            Session.Clear();
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();
        }
    }
}
