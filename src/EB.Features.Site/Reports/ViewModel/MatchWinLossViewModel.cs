namespace EB.Features.Site.Reports.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EB.Features.Reports.ViewModel;
    using EB.Features.Shared;
    using EB.Features.Shared.Configuration;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Resources;
    using EB.Features.Shared.Utilities;
    using EB.Features.Site.Reports.Request;
    using EB.Features.Site.Shared.ViewModels;
    using EB.Features.Site.Shared.ViewModels.Components;

    public class MatchWinLossReportViewModel
    {
        public MatchWinLossReportViewModel(
            MatchWinLossViewModel reportData,
            MatchWinLossQuery query,
            DateTime balanceUpDate,
            IEnumerable<SelectListItemExt> sportGroups,
            IEnumerable<SelectListItemExt> betTypes,            
            IEnumerable<SelectListItemExt> allBetTypes)
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
            ReportData = reportData;
            Query = query;
        }

        public MatchWinLossQuery Query { get; }

        public MatchWinLossViewModel ReportData { get; }

        public MatchWinLossFilterViewModel ReportFilterViewModel { get; }

        public static decimal TotalHeadlineStockPercent => 100;
    }
}