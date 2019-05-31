using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Model;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
using EB.Features.Shared.Utilies;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Reports.Request.HouseWinLoss;
using EB.Features.Site.Reports.ViewModel;
using EB.Features.Site.Reports.ViewModel.HouseWinLoss;
using EB.Features.Site.Shared.ViewModels;
using MediatR;

namespace EB.Features.Site.Reports
{
    public class ReportsHandler : ReportsBaseHandler, IRequestHandler<MatchWinLossQuery, MatchWinLossReportViewModel>,
        IRequestHandler<HouseWinLossQuery, HouseWinLossReportViewModel>
    {
        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly IMatchWinLossService matchWinLossService;
        private readonly IHouseWinLossService houseWinLossService;
        private readonly ReportDateService reportDateService;

        public ReportsHandler(
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            IMatchWinLossService matchWinLossService,
            ReportDateService reportDateService,
            IHouseWinLossService houseWinLossService)
        {
            this.houseWinLossService = houseWinLossService;
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.matchWinLossService = matchWinLossService;
            this.reportDateService = reportDateService;
        }

        public MatchWinLossReportViewModel Handle(MatchWinLossQuery message)
        {
            var sportGroups = sportGroupService.GetSportGroupsForSportbook();
            var allBetTypes = betTypeService.GetBetTypeGroupsBySport(0);
            var betTypes = allBetTypes.Where(betType => betType.SportTypeGroup == message.UserSelectedSportGroupId);
            var reportData = matchWinLossService.Get(new MatchWinLossParam
            {
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroup = message.UserSelectedSportGroupId,
                WinLossDate = message.Date
            });

            return new MatchWinLossReportViewModel(
                message,
                reportDateService.GetBalanceUpDay(),
                BuildSportGroupSelectListItems(sportGroups, message.UserSelectedSportGroupId),
                BuildBetTypeSelectListItems(betTypes, message.UserSelectedBetTypeId),
                reportData,
                BuildBetTypeSelectListItems(allBetTypes, 0));
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
                FromDate = message.Date,
                ToDate = SystemTime.Today
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