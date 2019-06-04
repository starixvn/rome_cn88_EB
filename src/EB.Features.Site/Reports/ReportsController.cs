namespace EB.Features.Site.Reports
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Reports.Request;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Shared;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using MediatR;

    public class ReportsController : BaseController
    {
        private const string ViewsPath = "~/Reports/Views";

        public ReportsController(IMediator mediator)
            : base(mediator)
        {
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.HouseWinLoss, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> HouseWinLoss(HouseWinLossQuery query)
          => await ViewAsync(query, $"{ViewsPath}/HouseWinLoss.cshtml");

        [NapAuth(FunctionKey = SiteFunctionKeys.HouseWinLoss, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> ExportHouseWinLoss(HouseWinLossExcelQuery query)
            => await ExcelView(query);

        [NapAuth(FunctionKey = SiteFunctionKeys.MatchWinLoss, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> MatchWinLoss(MatchWinLossQuery query)
            => await ViewAsync(query, $"{ViewsPath}/MatchWinLoss.cshtml");

        [NapAuth(FunctionKey = SiteFunctionKeys.MatchWinLoss, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> ExportMatchWinLoss(MatchWinLossExcelQuery query)
            => await ExcelView(query);
    }
}
