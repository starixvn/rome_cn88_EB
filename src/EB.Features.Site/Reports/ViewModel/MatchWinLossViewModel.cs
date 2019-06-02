using System;
using System.Collections.Generic;
using System.Linq;
using EB.Features.Reports.Model;
using EB.Features.Reports.ViewModel;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Utilies;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Shared.ViewModels;
using EB.Features.Site.Shared.ViewModels.Components;

namespace EB.Features.Site.Reports.ViewModel
{
    public class MatchWinLossReportViewModel
    {
        public MatchWinLossReportViewModel(
            MatchWinLossQuery query,
            DateTime balanceUpDate,
            IEnumerable<SelectListItemExt> sportGroups,
            IEnumerable<SelectListItemExt> betTypes,
            IEnumerable<MatchWinLossViewModel> reportData,
            IEnumerable<SelectListItemExt> allBetTypes = null)
        {
            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, query.UserSelectedSportGroupId);
            var betTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, query.UserSelectedBetTypeId);
            var allBetTypesSelectionViewModel = new SingleSelectionViewModel(allBetTypes, 0);

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItemExt
                {
                    Text = AppResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == query.UserSelectedLiveStatusId
                });
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, query.UserSelectedLiveStatusId);
            var datePickerViewModel = new DatePickerViewModel(balanceUpDate, SystemTime.Today, query.Date);

            ReportFilterViewModel = new MatchWinLossFilterViewModel(
                datePickerViewModel,
                query.IsTest,
                sportGroupSelectionViewModel,
                betTypeSelectionViewModel,
                liveStatusSelectionViewModel,
                allBetTypesSelectionViewModel);
            ReportData = reportData.Where(i => i.BetTypeGroup != BetTypeIdConstants.SystemParlay);

            MajorBetType = ReportData
                .Where(i => BetTypeIdConstants.MajorBetType.Contains(i.BetTypeGroup))
                .OrderBy(i => i.BetTypeGroup);
            MinorBetType = ReportData.Where(i => !BetTypeIdConstants.MajorBetType.Contains(i.BetTypeGroup));

            Parlay = reportData.FirstOrDefault(i => i.BetTypeGroup == BetTypeIdConstants.SystemParlay);
            TotalRow = Sum(reportData);
            Query = query;
        }

        public MatchWinLossQuery Query { get; }

        public IEnumerable<MatchWinLossViewModel> ReportData { get; }

        public MatchWinLossFilterViewModel ReportFilterViewModel { get; }

        public IEnumerable<MatchWinLossViewModel> MajorBetType { get; }

        public IEnumerable<MatchWinLossViewModel> MinorBetType { get; }

        public MatchWinLossViewModel Parlay { get; }

        public MatchWinLossModel TotalRow { get; }

        public bool IsShowTotalRow => ReportData.Any() || Parlay != null;

        public decimal TotalHeadlineStockPercent => 100;

        public MatchWinLossViewModel Sum(IEnumerable<MatchWinLossViewModel> matches)
        {
            var total = new MatchWinLossViewModel();
            foreach (var match in matches)
            {
                total = total.Add(match);
            }

            return total;
        }
    }
}