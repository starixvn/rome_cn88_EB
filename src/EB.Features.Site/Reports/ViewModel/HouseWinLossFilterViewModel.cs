using EB.Features.Site.Shared.ViewModels;
using EB.Features.Site.Shared.ViewModels.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EB.Features.Site.Reports.ViewModel
{
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

    }
}