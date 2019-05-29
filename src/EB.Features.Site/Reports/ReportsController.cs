using System.Threading.Tasks;
using System.Web.Mvc;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Reports.Request.HouseWinLoss;
using EB.Features.Site.Shared;
using MediatR;

namespace EB.Features.Site.Reports
{
    public class ReportsController : BaseController
    {
        private const string ViewsPath = "~/Reports/Views";

        public ReportsController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> HouseWinLoss(HouseWinLossQuery query)
          => await ViewAsync(query, $"{ViewsPath}/HouseWinLoss.cshtml");

        public async Task<ActionResult> ExportHouseWinLoss(HouseWinLossExcelQuery query)
            => await ExcelView(query);

        public async Task<ActionResult> MatchWinLoss(MatchWinLossQuery query)
            => await ViewAsync(query, $"{ViewsPath}/MatchWinLoss.cshtml");

        public async Task<ActionResult> ExportMatchWinLoss(MatchWinLossExcelQuery query)
            => await ExcelView(query);
    }
}
