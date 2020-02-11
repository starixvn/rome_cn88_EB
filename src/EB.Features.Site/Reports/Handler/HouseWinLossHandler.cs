namespace EB.Features.Site.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Resources;
    using EB.Features.Reports.Services;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Services;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class HouseWinLossHandler : ReportsBaseHandler,
        IRequestHandler<HouseWinLossQuery, HouseWinLossReportViewModel>,
        IRequestHandler<ReportLegendQuery, IEnumerable<string>>,
        IRequestHandler<CountHouseWinLossQuery, CountHouseWinLossViewModel>
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

        public IEnumerable<string> Handle(ReportLegendQuery message)
            => new List<string>
            {
                ReportResources.InaccurateDataLegend,
                ReportResources.CompareAccuratedDataLegend
            };

        public HouseWinLossReportViewModel Handle(HouseWinLossQuery message)
        {
            var sportGroups = sportGroupService.GetSportGroups();
            var allBetTypes = betTypeService.GetBetTypeGroupsBySport();
            message.UserSelectedSportGroupIds = GetValidSportGroupIds(sportGroups.Select(i => i.Id), message.UserSelectedSportGroupIds);

            var betTypes = betTypeService.GetBetTypeGroupsBySport(message.SelectedSportGroupIds).Distinct(new BetTypeGroupComparer());

            var reportData = houseWinLossService.Get(new HouseWinLossParam
            {
                HeadlineId = message.HeadlineId,
                CustId = message.CustId,
                Role = message.Role,
                Viewer = message.UserSelectedViewer,
                IsHistoricReport = message.IsHistoricReport,
                TicketStatusId = message.UserSelectedTicketStatusId,
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroups = message.UserSelectedSportGroupIds,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                Date = message.Date,
            });

            return new HouseWinLossReportViewModel(
                reportData,
                message,
                BuildSportGroupSelectListItems(sportGroups, message.SelectedSportGroupIds),
                BuildBetTypeSelectListItems(betTypes, message.UserSelectedBetTypeId),
                BuildBetTypeSelectListItems(allBetTypes, 0),
                userSession.Language);
        }

        public CountHouseWinLossViewModel Handle(CountHouseWinLossQuery message)
        {
            var sportGroups = sportGroupService.GetSportGroups();
            message.UserSelectedSportGroupIds = GetValidSportGroupIds(sportGroups.Select(i => i.Id), message.UserSelectedSportGroupIds);

            var model = houseWinLossService.CountUsers(new HouseWinLossParam
            {
                HeadlineId = message.HeadlineId,
                CustId = message.CustId,
                Role = message.Role,
                Viewer = message.UserSelectedViewer,
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

            return new CountHouseWinLossViewModel(model, message.Role);
        }
    }
}
