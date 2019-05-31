using System.Threading.Tasks;
using System.Web.Mvc;
using EB.Features.Site.Main.Requests;
using EB.Features.Site.Shared;
using MediatR;

namespace EB.Features.Site.Reports
{
    public class MainController : BaseController
    {
        private const string ViewsPath = "~/Main/Views";

        public MainController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(IndexQuery query)
            => await ViewAsync(query, $"{ViewsPath}/Index.cshtml");

        public async Task<ActionResult> MenuBar(MenuBarQuery query)
            => await ViewAsync(query, $"{ViewsPath}/MenuBar.cshtml");
    }
}
