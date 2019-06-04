namespace EB.Features.Site.Reports
{
    using System.Linq;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Services;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Services;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
    using MediatR;

    public class HouseWinLossHandler : ReportsBaseHandler, IRequestHandler<HouseWinLossQuery, HouseWinLossReportViewModel>
    {
        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly IHouseWinLossService houseWinLossService;
        private readonly ReportDateService reportDateService;

        public HouseWinLossHandler(
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            ReportDateService reportDateService,
            IHouseWinLossService houseWinLossService)
        {
            this.houseWinLossService = houseWinLossService;
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.reportDateService = reportDateService;
        }

        public HouseWinLossReportViewModel Handle(HouseWinLossQuery message)
        {
            var sportGroups = sportGroupService.GetSportGroupsForSportbook();
            var allBetTypes = betTypeService.GetBetTypeGroupsBySport(0);
            var betTypes = allBetTypes.Where(betType => betType.SportTypeGroup == message.UserSelectedSportGroupId);
            var reportData = houseWinLossService.Get(new HouseWinLossParam
            {
                IsSettled = message.UserSelectedTicketStatusId == TicketStatusConstants.Settled,
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroup = message.UserSelectedSportGroupId,
                FromDate = message.FromDate,
                ToDate = message.ToDate
            });

            return new HouseWinLossReportViewModel(
                message,
                reportDateService.GetBalanceUpDay(),
                BuildSportGroupSelectListItems(sportGroups, message.UserSelectedSportGroupId),
                BuildBetTypeSelectListItems(betTypes, message.UserSelectedBetTypeId),
                reportData,
                BuildBetTypeSelectListItems(allBetTypes, 0));
        }
    }
}