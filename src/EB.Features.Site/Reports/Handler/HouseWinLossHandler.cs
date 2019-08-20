namespace EB.Features.Site.Reports
{
    using System.Linq;
    using EB.Features.Reports.Extension;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Services;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Services;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class HouseWinLossHandler : ReportsBaseHandler, IRequestHandler<HouseWinLossQuery, HouseWinLossReportViewModel>
    {
        private readonly ISportGroupService sportGroupService;
        private readonly IBettypeGroupService betTypeService;
        private readonly IHouseWinLossService houseWinLossService;
        private readonly UserSession userSession;

        public HouseWinLossHandler(
            ISportGroupService sportGroupService,
            IBettypeGroupService betTypeService,
            IHouseWinLossService houseWinLossService,
            UserSession userSession)
        {
            this.houseWinLossService = houseWinLossService;
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.userSession = userSession;
        }

        public HouseWinLossReportViewModel Handle(HouseWinLossQuery message)
        {
            var sportGroups = sportGroupService.GetSportGroups();
            var allBetTypes = betTypeService.GetBetTypeGroupsBySport();
            message.UserSelectedSportGroupIds = GetValidSportGroupIds(sportGroups.Select(i => i.Id), message.UserSelectedSportGroupIds);

            var betTypes = betTypeService.GetBetTypeGroupsBySport(message.SelectedSportGroupIds)
                .GroupBy(i => i.Id)
                .Select(i => i.First());

            var reportData = houseWinLossService.Get(new HouseWinLossParam
            {
                IsHistoricReport = message.IsHistoricReport,
                TicketStatusId = message.UserSelectedTicketStatusId,
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroups = message.UserSelectedSportGroupIds,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                Date = message.Date
            });

            return new HouseWinLossReportViewModel(
                reportData,
                message,
                BuildSportGroupSelectListItems(sportGroups, message.SelectedSportGroupIds),
                BuildBetTypeSelectListItems(betTypes, message.UserSelectedBetTypeId),
                BuildBetTypeSelectListItems(allBetTypes, 0),
                userSession.Language);
        }
    }
}