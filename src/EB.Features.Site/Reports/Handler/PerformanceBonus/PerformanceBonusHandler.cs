using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EB.Features.Reports.Model;
using EB.Features.Reports.Params;
using EB.Features.Reports.Services;
using EB.Features.Shared.Utilities;
using EB.Features.Site.Reports.Request;
using EB.Features.Site.Reports.ViewModel.PerformanceBonus;
using EB.Features.Site.Shared.ViewModels;
using MediatR;

namespace EB.Features.Site.Reports.Handler.PerformanceBonus
{
    public class PerformanceBonusHandler : ReportsBaseHandler, IRequestHandler<PerformanceBonusQuery, PerformanceBonusViewModel>
    {
        private readonly AccountingPeriodService accountingPeriodService;
        private readonly IPerformanceBonusService performanceBonusService;

        public PerformanceBonusHandler(
            IPerformanceBonusService performanceBonusService,
            AccountingPeriodService accountingPeriodService)
        {
            this.accountingPeriodService = accountingPeriodService;
            this.performanceBonusService = performanceBonusService;
        }

        public PerformanceBonusViewModel Handle(PerformanceBonusQuery message)
        {
            var periods = accountingPeriodService.GetOldAccountingPeriods();
            message.UserSelectedPeriodId = string.IsNullOrEmpty(message.UserSelectedPeriodId)?
                accountingPeriodService.GetCurrentPeriod().Id :
                message.UserSelectedPeriodId;
            message.FromDate = message.FromDate == DateTime.MinValue ? accountingPeriodService.GetCurrentPeriod().FromDate : message.FromDate;
            message.ToDate = message.ToDate == DateTime.MinValue ? accountingPeriodService.GetCurrentPeriod().ToDate : message.ToDate;

            var periodSelectListItems = BuildPeriodSelectListItems(periods, message.UserSelectedPeriodId);
            var invalidSupersMessage = performanceBonusService.CheckValidSuper(message.UserNames);

            if (!string.IsNullOrEmpty(invalidSupersMessage))
            {
                return new PerformanceBonusViewModel(
                    message,
                    periodSelectListItems,
                    periods,
                    Enumerable.Empty<PerformanceBonusModel>(),
                    invalidSupersMessage);
            }

            var reports = performanceBonusService.Get(new PerformanceBonusParam
            {
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                ConjunctionId = message.UserSelectedConjunctionId,
                CurrencyId = message.UserSelectedCurrencyId,
                HeadlineStock = message.HeadlineStock,
                UserNames = message.UserNames,
                DividendRate = message.DividendRate
            });

            return new PerformanceBonusViewModel(message, periodSelectListItems, periods, reports);
        }

        protected internal static IEnumerable<SelectListItemExt> BuildPeriodSelectListItems(
            IEnumerable<AccountingPeriod> periods,
            string userSelectedPeriodId)
            => periods.Select(s => new SelectListItemExt
            {
                Text = $"{s.FromDateStr} ~ {s.ToDateStr}",
                Value = s.Id,
                Selected = s.Id == userSelectedPeriodId
            });
    }
}