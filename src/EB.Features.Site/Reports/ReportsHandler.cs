using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Shared.Model;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Services;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Reports.ViewModel;
using EB.Features.Site.Shared.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EB.Features.Site.Reports
{
    public class ReportsHandler: IRequestHandler<MatchWinLossQuery, MatchWinLossReportViewModel>
    {
        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly IMatchWinLossService matchWinLossService;
        private readonly ReportDateService reportDateService;

        public ReportsHandler(
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            IMatchWinLossService matchWinLossService,
            ReportDateService reportDateService)
        {
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.matchWinLossService = matchWinLossService;
            this.reportDateService = reportDateService;
        }

        public MatchWinLossReportViewModel Handle(MatchWinLossQuery message)
        {
            var sportGroups = sportGroupService.GetSportGroups();
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

        protected internal IEnumerable<SelectListItemExt> BuildSportGroupSelectListItems(
            IEnumerable<SportGroup> sportGroups,
            int userSelectedSportGroupId)
        {
            var items = sportGroups.Select(sportGroup => new SelectListItemExt
            {
                Text = sportGroup.DisplayName,
                Value = sportGroup.Id.ToString(),
                Selected = sportGroup.Id == userSelectedSportGroupId
            }).ToList();

            items.Insert(0, new SelectListItemExt { Text = AppResources.AllSports, Value = "0", Selected = userSelectedSportGroupId == 0 });

            return items;
        }

        protected internal IEnumerable<SelectListItemExt> BuildBetTypeSelectListItems(
            IEnumerable<BetTypeGroup> betTypes,
            int userSelectedBetTypeId)
        {
            var items = betTypes.Select(betType => new SelectListItemExt
            {
                SportTypeGroup = betType.SportTypeGroup,
                Text = betType.DisplayName,
                Value = betType.Id.ToString(),
                Selected = betType.Id == userSelectedBetTypeId
            }).ToList();

            items.Insert(0, new SelectListItemExt { Text = AppResources.AllBetTypes, Value = "0", Selected = userSelectedBetTypeId == 0 });

            return items;
        }
    }
}