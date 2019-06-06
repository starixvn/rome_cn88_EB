namespace EB.Features.Site.Reports.ViewModel.HouseWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using EB.Features.Reports;
    using EB.Features.Reports.ViewModel;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Extensions;
    using EB.Features.Shared.Resources;
    using EB.Features.Shared.Utilies;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Shared.ViewModels;
    using EB.Features.Site.Shared.ViewModels.Components;

    public class HouseWinLossReportViewModel
    {
        private readonly DateTime MinDate = new DateTime(2019, 1, 1);

        public HouseWinLossReportViewModel(
            HouseWinLossViewModel reportData,
            HouseWinLossQuery query,
            DateTime moveBetDate,
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
            var ticketStatuses = TicketStatusConstants.TicketStatuses
                .Select(s => new SelectListItemExt
                {
                    Text = AppResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == query.UserSelectedTicketStatusId
                });
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, query.UserSelectedLiveStatusId);
            var ticketStatusSelectionViewModel = new SingleSelectionViewModel(ticketStatuses, query.UserSelectedLiveStatusId);

            var datePickerViewModel = new DatePickerViewModel(MinDate, SystemTime.Today, query.Date);
            var fromDate = query.FromDate < moveBetDate ? query.FromDate : moveBetDate.FirstDateOfMonth();
            var toDate = query.ToDate < moveBetDate ? query.ToDate : moveBetDate;
            var dateRangePickerViewModel = new DateRangePickerViewModel(MinDate, moveBetDate, fromDate, toDate);

            ReportFilterViewModel = new HouseWinLossFilterViewModel(
                datePickerViewModel,
                dateRangePickerViewModel,
                query.IsTest,
                sportGroupSelectionViewModel,
                betTypeSelectionViewModel,
                liveStatusSelectionViewModel,
                ticketStatusSelectionViewModel,
                allBetTypesSelectionViewModel);
            ReportData = reportData;
            Query = query;
            MoveBetDate = moveBetDate;

            FromDate = Query.IsHistoricReport ? Query.FromDate : Query.Date;
            ToDate = Query.IsHistoricReport ? Query.ToDate : Query.Date;
        }

        private DateTime MoveBetDate { get; }

        public HouseWinLossQuery Query { get; }

        public HouseWinLossViewModel ReportData { get; }

        public HouseWinLossFilterViewModel ReportFilterViewModel { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string NoteMaxDateDescription => string.Format(ReportResources.NoteViewCurrentReport, MoveBetDate.ToShortDate());

        public string InternalLoginAction => HttpContext.Current.Response.ApplyAppPathModifier("~/InternalLogin/Index");

        public string GetDownlineLink()
        {
            var param = new Dictionary<string, object>
            {
                ["ReportType"] = Query.ReportType == 0 ? 1 : 2,
                ["DisplayMode"] = 1,
                ["IsOwner"] = true,
                ["CustLevelId"] = 4,
                ["FromDate"] = FromDate,
                ["ToDate"] = ToDate,
                ["UserSelectedSportGroupId"] = Query.UserSelectedSportGroupId,
                ["UserSelectedBetTypeId"] = Query.UserSelectedBetTypeId,
                ["UserSelectedLiveStatusId"] = Query.UserSelectedLiveStatusId
            };

            return "~/Reports/Wager?" + string.Join("&", param.Select(kp => kp.Key + "=" + kp.Value));
        }
    }
}
