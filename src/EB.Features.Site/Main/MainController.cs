namespace EB.Features.Site.Reports
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EB.Features.Site.Main.Requests;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class MainController : BaseController
    {
        private const string ViewsPath = "~/Main/Views";

        public MainController(IMediator mediator, UserSession userSession)
            : base(mediator, userSession)
        {
        }

        public async Task<ActionResult> Index(IndexQuery query)
            => await ViewAsync(query, $"{ViewsPath}/Index.cshtml");

        public async Task<ActionResult> MenuBar(MenuBarQuery query)
            => await ViewAsync(query, $"{ViewsPath}/MenuBar.cshtml");
    }
}
