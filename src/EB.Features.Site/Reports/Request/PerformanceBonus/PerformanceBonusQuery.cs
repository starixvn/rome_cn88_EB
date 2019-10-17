namespace EB.Features.Site.Reports.Request
{
    using System;
    using EB.Features.Reports.Constants;
    using EB.Features.Shared.Utilities;
    using EB.Features.Site.Reports.ViewModel.PerformanceBonus;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class PerformanceBonusQuery : ReportBaseQuery, IRequest<PerformanceBonusViewModel>
    {
        public PerformanceBonusQuery(UserSession currentSession)
            : base(currentSession)
        {
            UserSelectedCurrencyId = CurrencyConstant.CY;
            UserSelectedConjunctionId = ConjunctionConstant.And;
            HeadlineStock = 1000000;
            FromDate = DateTime.MinValue;
            ToDate = DateTime.MinValue;
        }

        public string UserSelectedPeriodId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int UserSelectedCurrencyId { get; set; }

        public decimal HeadlineStock { get; set; }

        public int UserSelectedConjunctionId { get; set; }

        public string UserNames { get; set; }

        public decimal BonusRate { get; set; }
    }
}