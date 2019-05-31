using EB.Features.Reports.Model;
using EB.Features.Shared.Constants;
using EB.Features.Shared.Resources;
using EB.Features.Shared.Utilies;
using EB.Features.Site.Reports.Request.HouseWinLoss;
using EB.Features.Site.Shared.ViewModels;
using EB.Features.Site.Shared.ViewModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EB.Features.Site.Reports.ViewModel.HouseWinLoss
{
    public class HouseWinLossReportViewModel
    {
        public HouseWinLossReportViewModel(
            HouseWinLossQuery query,
            DateTime balanceUpDate,
            IEnumerable<SelectListItemExt> sportGroups,
            IEnumerable<SelectListItemExt> betTypes,
            IEnumerable<HouseWinLossModel> reportData,
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
            var ticketStatuses = TicketStatusConstants.TicketStatuses
                .Select(s => new SelectListItemExt
                {
                    Text = AppResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == query.UserSelectedTicketStatusId
                });
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, query.UserSelectedLiveStatusId);
            var ticketStatusSelectionViewModel = new SingleSelectionViewModel(ticketStatuses, query.UserSelectedLiveStatusId);
            var datePickerViewModel = new DatePickerViewModel(balanceUpDate, SystemTime.Today, query.Date);

            ReportFilterViewModel = new HouseWinLossFilterViewModel(
                datePickerViewModel,
                query.IsTest,
                sportGroupSelectionViewModel,
                betTypeSelectionViewModel,
                liveStatusSelectionViewModel,
                ticketStatusSelectionViewModel,
                allBetTypesSelectionViewModel);
            ReportData = reportData;

            TotalRow = Sum(reportData);
            Query = query;
        }

        public HouseWinLossQuery Query { get; }

        public IEnumerable<HouseWinLossModel> ReportData { get; }

        public HouseWinLossFilterViewModel ReportFilterViewModel { get; }

        public HouseWinLossModel TotalRow { get; }

        public bool IsShowTotalRow => ReportData.Any();

        public HouseWinLossModel Sum(IEnumerable<HouseWinLossModel> matches)
        {
            var total = new HouseWinLossModel();
            foreach (var match in matches)
            {
                total = total.Add(match);
            }

            return total;
        }
    }
}