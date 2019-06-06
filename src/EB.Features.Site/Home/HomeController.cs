using System.Web.Mvc;
using Fanex.NAP.Core.Web.Attribute;

namespace EB.Features.Site.Reports
{
    [NapSuperAuth]
    public class HomeController : Controller
    {
        public ActionResult Index()
            => View("~/Home/Views/Index.cshtml");
    }
}
