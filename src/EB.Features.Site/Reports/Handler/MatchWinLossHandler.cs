namespace EB.Features.Site.Reports
{
    using System.Linq;
    using EB.Features.Reports.Params;
    using EB.Features.Reports.Services;
    using EB.Features.Shared.Services;
    using EB.Features.Site.Reports.Request;
    using EB.Features.Site.Reports.ViewModel;
    using MediatR;

    public class MatchWinLossHandler : ReportsBaseHandler, IRequestHandler<MatchWinLossQuery, MatchWinLossReportViewModel>
    {
        private readonly ISportGroupService sportGroupService;
        private readonly IBettypeGroupService betTypeService;
        private readonly IMatchWinLossService matchWinLossService;
        private readonly IReportDateService reportDateService;

        public MatchWinLossHandler(
            ISportGroupService sportGroupService,
            IBettypeGroupService betTypeService,
            IMatchWinLossService matchWinLossService,
            IReportDateService reportDateService)
        {
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.matchWinLossService = matchWinLossService;
            this.reportDateService = reportDateService;
        }

        public MatchWinLossReportViewModel Handle(MatchWinLossQuery message)
        {
            var sportGroups = sportGroupService.GetSportGroups();
            var allBetTypes = betTypeService.GetBetTypeGroupsBySport();
            message.UserSelectedSportGroupIds = GetValidSportGroupIds(sportGroups.Select(i => i.Id), message.UserSelectedSportGroupIds);

            var betTypes = betTypeService.GetBetTypeGroupsBySport(message.SelectedSportGroupIds);

            var reportData = matchWinLossService.Get(new MatchWinLossParam
            {
                BetTypeGroup = message.UserSelectedBetTypeId,
                IsExcludedTest = message.IsTest,
                IsLive = message.UserSelectedLiveStatusId,
                SportTypeGroups = message.UserSelectedSportGroupIds,
                WinLossDate = message.Date
            });

            return new MatchWinLossReportViewModel(
                reportData,
                message,
                reportDateService.GetBalanceUpDay(),
                BuildSportGroupSelectListItems(sportGroups, message.SelectedSportGroupIds),
                BuildBetTypeSelectListItems(betTypes, message.UserSelectedBetTypeId),
                BuildBetTypeSelectListItems(allBetTypes, 0));
        }
    }
}