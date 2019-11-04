namespace EB.Features.Site.Reports.ViewModel
{
    using EB.Features.Reports;
    using EB.Features.Reports.Resources;
    using EB.Features.Site.Shared.ViewModels.Components;

    public class HouseWinLossFilterViewModel
    {
        public HouseWinLossFilterViewModel(
            DatePickerViewModel datePickerViewModel,
            DateRangePickerViewModel dateRangePickerViewModel,
            bool isTest,
            SingleSelectionViewModel viewerSelectionViewModel,
            SportGroupMultiSelectionViewModel sportGroupMultiSelectionViewModel,
            SingleSelectionViewModel betTypeSelectionViewModel,
            SingleSelectionViewModel liveStatusSelectionViewModel,
            SingleSelectionViewModel ticketStatusSelectionViewModel,
            SingleSelectionViewModel allBetTypesSelectionViewModel)
        {
            ViewerSelectionViewModel = viewerSelectionViewModel;
            BetTypeSelectionViewModel = betTypeSelectionViewModel;
            LiveStatusSelectionViewModel = liveStatusSelectionViewModel;
            TicketStatusSelectionViewModel = ticketStatusSelectionViewModel;
            AllBetTypesSelectionViewModel = allBetTypesSelectionViewModel;
            SportGroupMultiSelectionViewModel = sportGroupMultiSelectionViewModel;
            DatePickerViewModel = datePickerViewModel;
            DateRangePickerViewModel = dateRangePickerViewModel;
            IsTest = isTest;
        }

        public SingleSelectionViewModel ViewerSelectionViewModel { get; }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

        public SingleSelectionViewModel TicketStatusSelectionViewModel { get; }

        public SingleSelectionViewModel AllBetTypesSelectionViewModel { get; }

        public SportGroupMultiSelectionViewModel SportGroupMultiSelectionViewModel { get; }

        public DatePickerViewModel DatePickerViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public bool IsTest { get; }

        public static string[] MonthNames => new[]
        {
            ReportResources.January,
            ReportResources.February,
            ReportResources.March,
            ReportResources.April,
            ReportResources.May,
            ReportResources.June,
            ReportResources.July,
            ReportResources.August,
            ReportResources.September,
            ReportResources.October,
            ReportResources.November,
            ReportResources.December
        };
    }
}