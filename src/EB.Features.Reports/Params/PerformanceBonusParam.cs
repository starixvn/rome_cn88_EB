using System;

namespace EB.Features.Reports.Params
{
    public class PerformanceBonusParam
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int CurrencyId { get; set; }

        public decimal HeadlineStock { get; set; }

        public int ConjunctionId { get; set; }

        public string UserNames { get; set; }

        public decimal DividendRate { get; set; }
    }
}
