namespace EB.Features.Site.Reports.ViewModel.HouseWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using EB.Features.Reports;
    using EB.Features.Reports.ViewModel;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Extensions;
    using EB.Features.Shared.Model;
    using EB.Features.Shared.Resources;
    using EB.Features.Shared.Utilities;
    using EB.Features.Site.Reports.Request.HouseWinLoss;
    using EB.Features.Site.Shared.Helpers;
    using EB.Features.Site.Shared.ViewModels;
    using EB.Features.Site.Shared.ViewModels.Components;

    public class HouseWinLossReportViewModel
    {
        private readonly DateTime MinDate = new DateTime(2019, 1, 1);

        public HouseWinLossReportViewModel(
            HouseWinLossViewModel reportData,
            HouseWinLossQuery query,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItemExt> betTypes,
            IEnumerable<SelectListItemExt> allBetTypes,
            string languageCode)
        {
            var sportGroupSelectionViewModel = new SportGroupMultiSelectionViewModel(sportGroups, query.UserSelectedSportGroupIds, query.CurrentSession.Language);
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
            var fromDate = query.FromDate <= SystemTime.Yesterday ? query.FromDate : SystemTime.Yesterday.FirstDateOfMonth();
            var toDate = query.ToDate < SystemTime.Yesterday ? query.ToDate : SystemTime.Yesterday;
            var dateRangePickerViewModel = new DateRangePickerViewModel(MinDate, SystemTime.Yesterday, fromDate, toDate);

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
            LanguageCode = languageCode;
        }

        public HouseWinLossQuery Query { get; }

        public HouseWinLossViewModel ReportData { get; }

        public HouseWinLossFilterViewModel ReportFilterViewModel { get; }

        public static string NoteMaxDateDescription => string.Format(ReportResources.NoteViewCurrentReport, SystemTime.Yesterday.ToShortDate());

        public static string InternalLoginAction => HttpContext.Current.Response.ApplyAppPathModifier("~/InternalLogin/Index");

        public static string GetDownlineLink()
        {
            var param = new Dictionary<string, object>
            {                
                ["DisplayMode"] = 1,
                ["IsSetMenu"] = true
            };

            return "~/Reports/Index?" + string.Join("&", param.Select(kp => kp.Key + "=" + kp.Value));
        }

        public string LanguageCode { get; }        
    }
}
