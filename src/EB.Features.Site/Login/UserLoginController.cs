namespace EB.Features.Site.Login
{
    using System.Web.Mvc;
    using System.Web.Security;
    using EB.Features.Site.Login.Requests;
    using EB.Features.Site.Shared.Helpers;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Configuration;
    using MediatR;

    [AllowAnonymous]
    public class UserLoginController : Controller
    {
        private readonly IMediator mediator;
        private readonly UserSession userSession;

        public UserLoginController(IMediator mediator, UserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        public ActionResult SSO(SSOQuery message)
        {
            message.SessionId = Session.SessionID;
            message.SiteId = NapSettings.Settings.SiteId;

            var viewModel = mediator.Send(message).ConfigureAwait(false).GetAwaiter().GetResult();

            if (string.IsNullOrWhiteSpace(viewModel.ErrorMessage))
            {
                AuthInfoHelper.CreateAuthInfo(viewModel.UserInfo, userSession.OnUserId, HttpContext);

                return Redirect(FormsAuthentication.DefaultUrl);
            }

            return View("~/Login/Views/Index.cshtml", viewModel);
        }
    }
}