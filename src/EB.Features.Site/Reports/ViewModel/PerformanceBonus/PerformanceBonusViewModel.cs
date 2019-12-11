using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EB.Features.Reports;
using EB.Features.Reports.Constants;
using EB.Features.Reports.Extension;
using EB.Features.Reports.Model;
using EB.Features.Reports.Resources;
using EB.Features.Shared.Utilities;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Shared.ViewModels;
using EB.Features.Site.Shared.ViewModels.Components;

namespace EB.Features.Site.Reports.ViewModel.PerformanceBonus
{
    public class PerformanceBonusViewModel
    {
        public PerformanceBonusViewModel(
            PerformanceBonusQuery query,
            IEnumerable<SelectListItemExt> periodItems,
            IEnumerable<AccountingPeriod> periods,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<PerformanceBonusModel> reports,
            string errorMessage = "")
        {
            Query = query;
            var conjunctions = ConjunctionConstant.List
                .Select(s => new SelectListItemExt
                {
                    Text = ReportResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == Query.UserSelectedConjunctionId
                });
            var currencies = CurrencyConstant.List
                .Select(s => new SelectListItemExt
                {
                    Text = ReportResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == Query.UserSelectedCurrencyId
                });

            PeriodSelectionViewModel = new PeriodSelectionViewModel(periodItems, query.UserSelectedPeriodId);
            CurrencySelectionViewModel = new SingleSelectionViewModel(currencies, query.UserSelectedCurrencyId);
            ConjunctionSelectionViewModel = new SingleSelectionViewModel(conjunctions, query.UserSelectedConjunctionId);
            DateRangePickerViewModel = new DateRangePickerViewModel(SystemTime.MinDate, SystemTime.Yesterday, query.FromDate, query.ToDate, null);
            SportGroupMultiSelectionViewModel = new SportGroupMultiSelectionViewModel(sportGroups, query.UserSelectedSportGroupIds, query.CurrentSession.Language);
            ErrorMessage = errorMessage;
            Reports = reports;
            Total = Reports.Sum();
            Periods = periods;
        }

        public PerformanceBonusQuery Query { get; }

        public IEnumerable<PerformanceBonusModel> Reports { get; }

        public string ErrorMessage { get; }

        public PeriodSelectionViewModel PeriodSelectionViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public SingleSelectionViewModel CurrencySelectionViewModel { get; }

        public SingleSelectionViewModel ConjunctionSelectionViewModel { get; }

        public SportGroupMultiSelectionViewModel SportGroupMultiSelectionViewModel { get; }

        public PerformanceBonusModel Total { get; }

        public IEnumerable<AccountingPeriod> Periods { get; }
    }
}