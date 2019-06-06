using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EB.Features.Main.Services;
using EB.Features.Shared.Resources;
using Fanex.Common.Extensions;
using Fanex.NAP.Core.Configuration;
using Fanex.NAP.Core.Models;
using Fanex.NAP.Core.Models.ParameterObject;
using Fanex.NAP.Core.Service.Interface;
using Fanex.NAP.Core.Web.Auth;
using Fanex.NAP.Core.Web.Context;
using Fanex.NAP.Core.Web.Session;

namespace EB.Features.Site
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUserService userService;
        private readonly ICurrentSession currentSession;
        private readonly IGroupService groupService;
        private readonly ICurrentContext currentContext;
        private readonly IUnderMainternanceService maintenanceService;
        private readonly ISecurityService securityService;

        private readonly HashAlgorithm hashAlgorithm;

        public LoginController(
            IUserService userService,
            IGroupService groupService,
            ICurrentContext currentContext,
            ISecurityService securityService,
            IUnderMainternanceService maintenanceService,
            ICurrentSession currentSession)
        {
            this.groupService = groupService;
            this.userService = userService;
            this.currentSession = currentSession;
            this.currentContext = currentContext;
            this.securityService = securityService;
            this.maintenanceService = maintenanceService;
            hashAlgorithm = new SHA256Managed();
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

            ClearAllCookie();

            return View("~/Login/Views/Index.cshtml");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string username, string password, string returnUrl)
        {
            if (!username.IsNullOrTrimEmpty() && !password.IsNullOrTrimEmpty())
            {
                var user = userService.Get(username.Trim());

                if (user != null)
                {
                    var hashPassword = GetHash(user.Password);

                    if (password == hashPassword)
                    {
                        if (CheckUserSitePermission(user))
                        {
                            CheckIn(user);
                            CreateUserMenu(user);
                            CreateAuthInfo(user);

                            var redirectUrl = string.IsNullOrEmpty(returnUrl) ||
                                              returnUrl.IndexOf("Logout", StringComparison.CurrentCultureIgnoreCase) >=
                                              0
                                ? FormsAuthentication.DefaultUrl
                                : returnUrl;
                            return Redirect(redirectUrl);
                        }
                        ViewBag.Message = SiteResources.You_have_no_authorization;
                    }
                    else
                    {
                        ViewBag.Message = SiteResources.Invalid_Username_or_Password;
                    }
                }
                else
                {
                    ViewBag.Message = SiteResources.Invalid_Username_or_Password;
                }
            }
            else
            {
                ViewBag.Message = SiteResources.Invalidinput;
            }

            return View("~/Login/Views/Index.cshtml");
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
                if (Request.QueryString["UMToken"] != null)
                {
                    Session["UMToken"] = Request.QueryString["UMToken"];
                }

                FormsAuthentication.RedirectToLoginPage();
            }

            return View("~/Login/Views/UnderMaintenance.cshtml", downTime);
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

        private void CreateUserMenu(User user)
        {
            currentSession.MenuItemList = (List<MenuItem>)userService.GetMenuItem(
                user.Id,
                NapSettings.Settings.SiteId,
                NapSettings.Settings.Environment,
                true);
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
            currentSession.Level = userService.GetUserLevel(user.Id, NapSettings.Settings.SiteId);
            currentSession.UserId = user.Id;
            currentSession.Username = user.Username;
            currentSession.OnUserId = onUserId;
            currentSession.SessionId = Session.SessionID;
        }

        private bool CheckUserSitePermission(User user)
        {
            var group = user.Group == null ? null : groupService.Get(user.Group.Id);
            var siteLevelList = userService.GetSiteLevel(user.Id, NapSettings.Settings.Environment);
            var menuList = userService.GetMenuItem(user.Id, NapSettings.Settings.SiteId);

            // gets all sites of logging-in user
            return (group == null || group.IsActive) &&
                   siteLevelList.Any(x => x.Site.Id == NapSettings.Settings.SiteId && x.IsActive &&
                   menuList.Any());
        }

        private void ClearAllCookie()
        {
            var limit = Request.Cookies.Count;
            for (var i = 0; i < limit; i++)
            {
                var cookieName = Request.Cookies[i].Name;
                var cookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(cookie);
            }
        }

        private string GetHash(string input)
        {
            var encrypted = string.Empty;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var hashData = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                encrypted = BitConverter.ToString(hashData).Replace("-", string.Empty).ToLower();
            }

            return encrypted;
        }
    }
}