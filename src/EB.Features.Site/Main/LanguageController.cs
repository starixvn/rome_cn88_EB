namespace EB.Features.Site.Main
{
    using System.Web.Mvc;
    using EB.Features.Site.Main.Requests;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    [AllowAnonymous]
    public class LanguageController : Controller
    {
        private readonly IMediator mediator;
        private readonly UserSession userSession;

        public LanguageController(IMediator mediator, UserSession userSession)
        {
            this.mediator = mediator;
            this.userSession = userSession;
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Change(string languageCode)
        {
            var viewModel = mediator.Send(new ChangeLanguageCommand(userSession, languageCode));

            return Json(viewModel);
        }
    }
}
