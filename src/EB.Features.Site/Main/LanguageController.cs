namespace EB.Features.Site.Main
{
    using System.Web.Mvc;
    using EB.Features.Site.Main.Requests;
    using MediatR;

    [AllowAnonymous]
    public class LanguageController : Controller
    {
        private readonly IMediator mediator;

        public LanguageController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Change(string languageCode)
        {
            var viewModel = mediator.Send(new ChangeLanguageCommand(languageCode));

            return Json(viewModel);
        }
    }
}
