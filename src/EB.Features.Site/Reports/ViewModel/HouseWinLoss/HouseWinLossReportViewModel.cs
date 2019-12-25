namespace EB.Features.Site.Reports.ViewModel.HouseWinLoss
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using EB.Features.Reports.Resources;
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
        private readonly int DayRange = 90;

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
            var viewers = ViewerConstants.Viewers
                .Select(s => new SelectListItemExt
                {
                    Text = AppResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == query.UserSelectedViewer
                });

            var viewerSelectionViewModel = new SingleSelectionViewModel(viewers, query.UserSelectedViewer);
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, query.UserSelectedLiveStatusId);
            var ticketStatusSelectionViewModel = new SingleSelectionViewModel(ticketStatuses, query.UserSelectedLiveStatusId);

            var datePickerViewModel = new DatePickerViewModel(SystemTime.MinDate, SystemTime.Today, query.Date);
            var fromDate = query.FromDate <= SystemTime.Yesterday ? query.FromDate : SystemTime.Yesterday.AddDays(-DayRange);
            var toDate = query.ToDate < SystemTime.Yesterday ? query.ToDate : SystemTime.Yesterday;
            var dateRangePickerViewModel = new DateRangePickerViewModel(SystemTime.MinDate, SystemTime.Yesterday, fromDate, toDate, DayRange);

            ReportFilterViewModel = new HouseWinLossFilterViewModel(
                datePickerViewModel,
                dateRangePickerViewModel,
                query.IsTest,
                viewerSelectionViewModel,
                sportGroupSelectionViewModel,
                betTypeSelectionViewModel,
                liveStatusSelectionViewModel,
                ticketStatusSelectionViewModel,
                allBetTypesSelectionViewModel);
            ReportData = reportData;
            Query = query;
            LanguageCode = languageCode;
            breadcrumbLevel = GetBreadcrumbLevel();
        }

        public decimal ProfitMargin
            => Query.Role.IsSuperLevel() && ReportData.Total.SuperStock != 0
            ? ReportData.Total.SuperResult / ReportData.Total.SuperStock * 100
            : 0;

        private IEnumerable<CustInfo> breadcrumbLevel { get; }

        public HouseWinLossQuery Query { get; }

        public HouseWinLossViewModel ReportData { get; }

        public HouseWinLossFilterViewModel ReportFilterViewModel { get; }

        public static string NoteMaxDateDescription => string.Format(ReportResources.NoteViewCurrentReport, SystemTime.Yesterday.ToShortDate());

        public static string InternalLoginAction => HttpContext.Current.Response.ApplyAppPathModifier("~/InternalLogin/Index");

        public string GetDownlineLink(int custId, string custName, byte roleId, int headlineId)
        {
            var breadscrumbToDonwline = breadcrumbLevel.ToList();
            breadscrumbToDonwline.Add(new CustInfo(Query.CustId, custName, Query.RoleId));

            var param = new Dictionary<string, object>
            {
                ["HeadlineId"] = headlineId,
                ["RoleId"] = roleId,
                ["UserSelectedViewer"] = Query.UserSelectedViewer,
                ["UserSelectedSportGroupIds"] = Query.UserSelectedSportGroupIds,
                ["UserSelectedBetTypeId"] = Query.UserSelectedBetTypeId,
                ["UserSelectedLiveStatusId"] = Query.UserSelectedLiveStatusId,
                ["UserSelectedTicketStatusId"] = Query.UserSelectedTicketStatusId,
                ["Date"] = Query.Date,
                ["IsTest"] = Query.IsTest,
                ["IsHistoricReport"] = Query.IsHistoricReport,
                ["FromDate"] = Query.FromDate,
                ["ToDate"] = Query.ToDate,
                ["CustId"] = custId,
                ["CustName"] = custName,
                ["BreadcrumbLevelJson"] = JsonHelper.ToJsonFunc(breadscrumbToDonwline)
            };

            return HttpContext.Current.Response.ApplyAppPathModifier("~/Reports/HouseWinLoss?" + string.Join("&", param.Select(kp => kp.Key + "=" + kp.Value)));
        }

        public string GetBetlistLink(int custId, string custName)
        {
            var param = new Dictionary<string, object>
            {
                ["FromDate"] = Query.IsHistoricReport ? Query.FromDate : Query.Date,
                ["ToDate"] = Query.IsHistoricReport ? Query.ToDate : Query.Date,
                ["CustId"] = custId,
                ["CustName"] = custName,
                ["UserSelectedSportGroupId"] = Query.UserSelectedSportGroupIds,
                ["UserSelectedBetTypeId"] = Query.UserSelectedBetTypeId,
                ["UserSelectedLiveStatusId"] = Query.UserSelectedLiveStatusId,
                ["Type"] = Query.UserSelectedTicketStatusId,
                ["ExcludeDataTest"] = Query.IsTest,
            };

            return "~/BetLists/BetListOfEBReport?" + string.Join("&", param.Select(kp => kp.Key + "=" + kp.Value));
        }

        public static string GetReportLink()
        {
            var param = new Dictionary<string, object>
            {
                ["DisplayMode"] = 1,
                ["IsSetMenu"] = true
            };

            return "~/Reports/Index?" + string.Join("&", param.Select(kp => kp.Key + "=" + kp.Value));
        }

        public string LanguageCode { get; }

        public BreadcrumbLevelViewModel BreadcrumbLevelViewModel
            => new BreadcrumbLevelViewModel(breadcrumbLevel);

        private IEnumerable<CustInfo> GetBreadcrumbLevel()
        {
            var breadcrumb = new List<CustInfo>();

            foreach (var custInfo in Query.BreadcrumbLevel)
            {
                if (custInfo.RoleId == Query.RoleId)
                {
                    return breadcrumb;
                }

                breadcrumb.Add(custInfo);
            }

            return breadcrumb;
        }
    }
}
