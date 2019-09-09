namespace EB.Features.Site.Reports.ViewModel
{
    using EB.Features.Site.Shared.ViewModels.Components;

    public class MatchWinLossFilterViewModel
    {
        public MatchWinLossFilterViewModel(
            DatePickerViewModel datePickerViewModel,
            bool isTest,
            SportGroupMultiSelectionViewModel sportGroupMultiSelectionViewModel,
            SingleSelectionViewModel betTypeSelectionViewModel,
            SingleSelectionViewModel liveStatusSelectionViewModel,
            SingleSelectionViewModel allBetTypesSelectionViewModel = null)
        {
            BetTypeSelectionViewModel = betTypeSelectionViewModel;
            LiveStatusSelectionViewModel = liveStatusSelectionViewModel;
            AllBetTypesSelectionViewModel = allBetTypesSelectionViewModel;
            SportGroupMultiSelectionViewModel = sportGroupMultiSelectionViewModel;
            DatePickerViewModel = datePickerViewModel;
            IsTest = isTest;
        }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

        public SingleSelectionViewModel AllBetTypesSelectionViewModel { get; }

        public SportGroupMultiSelectionViewModel SportGroupMultiSelectionViewModel { get; }

        public DatePickerViewModel DatePickerViewModel { get; }

        public bool IsTest { get; }

    }
}