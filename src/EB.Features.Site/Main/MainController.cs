using EB.Features.Main.Services;
using EB.Features.Site.Main.Requests;
using EB.Features.Site.Main.ViewModel;
using EB.Features.Site.Shared;
using Fanex.NAP.Core.Web.Attribute;
using Fanex.NAP.Core.Web.Session;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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
