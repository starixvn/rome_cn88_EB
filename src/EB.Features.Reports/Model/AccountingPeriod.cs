using System;
using EB.Features.Shared.Utilities;

namespace EB.Features.Reports.Model
{
    public class AccountingPeriod
    {
        public AccountingPeriod()
        {
        }

        public AccountingPeriod(
            int periodId,
            int year,
            DateTime fromDate,
            DateTime toDate)
        {
            PeriodId = periodId;
            Year = year;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public string Id => $"{Year}_{PeriodId}";

        public int PeriodId { get; }

        public int Year { get; }

        public DateTime FromDate { get; }

        public string FromDateStr => FromDate.ToString("yyyy/MM/dd");

        public DateTime ToDate { get; }

        public string ToDateStr => ToDate.ToString("yyyy/MM/dd");

        public bool IsCurrentPeriod => FromDate <= SystemTime.Today && SystemTime.Today <= ToDate;
    }
}
