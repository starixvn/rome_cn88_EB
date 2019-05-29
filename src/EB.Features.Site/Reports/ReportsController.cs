using EB.Features.Site.Reports.Request;
using EB.Features.Site.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EB.Features.Site.Reports
{
    public class ReportsController : BaseController
    {
        private const string ViewsPath = "~/Reports/Views";

        public ReportsController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult HouseWinLoss()
        {
            return View("~/Reports/Views/HouseWinLoss.cshtml");
        }

        public async Task<ActionResult> MatchWinLoss(MatchWinLossQuery query)
            => await ViewAsync(query, $"{ViewsPath}/MatchWinLoss.cshtml");

        public async Task<ActionResult> ExportMatchWinLoss(MatchWinLossExcelQuery query)
            => await ExcelView(query);
    }
}
