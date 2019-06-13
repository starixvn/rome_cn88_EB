namespace EB.Features.Site.Reports.ViewModel
{
    using EB.Features.Reports;
    using EB.Features.Site.Shared.ViewModels.Components;

    public class HouseWinLossFilterViewModel
    {
        public HouseWinLossFilterViewModel(
            DatePickerViewModel datePickerViewModel,
            DateRangePickerViewModel dateRangePickerViewModel,
            bool isTest,
            SingleSelectionViewModel sportGroupSelectionViewModel,
            SingleSelectionViewModel betTypeSelectionViewModel,
            SingleSelectionViewModel liveStatusSelectionViewModel,
            SingleSelectionViewModel ticketStatusSelectionViewModel,
            SingleSelectionViewModel allBetTypesSelectionViewModel = null)
        {
            BetTypeSelectionViewModel = betTypeSelectionViewModel;
            LiveStatusSelectionViewModel = liveStatusSelectionViewModel;
            TicketStatusSelectionViewModel = ticketStatusSelectionViewModel;
            AllBetTypesSelectionViewModel = allBetTypesSelectionViewModel;
            SportGroupSelectionViewModel = sportGroupSelectionViewModel;
            DatePickerViewModel = datePickerViewModel;
            DateRangePickerViewModel = dateRangePickerViewModel;
            IsTest = isTest;
        }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

        public SingleSelectionViewModel TicketStatusSelectionViewModel { get; }

        public SingleSelectionViewModel AllBetTypesSelectionViewModel { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public DatePickerViewModel DatePickerViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public bool IsTest { get; }

        public string[] MonthNames => new[]
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