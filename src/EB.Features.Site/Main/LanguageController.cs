namespace EB.Features.Site.Main
{
    using System.Threading.Tasks;
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

        [ChildActionOnly]
        public async Task<ActionResult> List(AllLanguagesQuery query)
        {
            var viewModel = await mediator.Send(query);

            return PartialView("~/Main/Views/LanguageList.cshtml", viewModel);
        }
    }
}
