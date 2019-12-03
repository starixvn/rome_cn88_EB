namespace EB.Features.Site.Reports
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Reports.Request;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using MediatR;

    public class ReportsController : BaseController
    {
        private const string ViewsPath = "~/Reports/Views";

        public ReportsController(IMediator mediator, UserSession userSession)
            : base(mediator, userSession)
        {
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.HouseWinLoss, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> HouseWinLoss(HouseWinLossQuery query)
          => await ViewAsync(query, $"{ViewsPath}/HouseWinLoss.cshtml");

        [NapAuth(FunctionKey = SiteFunctionKeys.HouseWinLoss, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> CountHouseWinLoss(CountHouseWinLossQuery query)
          => await ViewAsync(query, $"{ViewsPath}/HouseWinLoss/CountUsers.cshtml");

        [NapAuth(FunctionKey = SiteFunctionKeys.HouseWinLoss, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> ExportHouseWinLoss(HouseWinLossExcelQuery query)
            => await ExcelView(query);

        public async Task<ActionResult> LegendHouseWinLoss(ReportLegendQuery query)
            => await ViewAsync(query, $"{ViewsPath}/ReportLegend.cshtml");

        [NapAuth(FunctionKey = SiteFunctionKeys.MatchWinLoss, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> MatchWinLoss(MatchWinLossQuery query)
            => await ViewAsync(query, $"{ViewsPath}/MatchWinLoss.cshtml");

        [NapAuth(FunctionKey = SiteFunctionKeys.MatchWinLoss, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> ExportMatchWinLoss(MatchWinLossExcelQuery query)
            => await ExcelView(query);

        [NapAuth(FunctionKey = SiteFunctionKeys.PerformanceBonus, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> PerformanceBonus(PerformanceBonusQuery query)
            => await ViewAsync(query, $"{ViewsPath}/PerformanceBonus.cshtml");

        [NapAuth(FunctionKey = SiteFunctionKeys.PerformanceBonus, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> ExportPerformanceBonus(PerformanceBonusExcelQuery query)
            => await ExcelView(query);

    }
}
