namespace EB.Features.Site.Shared.ViewModels.Components
{
    using System;

    public class DateRangePickerViewModel
    {
        public DateRangePickerViewModel(
            DateTime minDate,
            DateTime maxDate,
            DateTime fromDate,
            DateTime toDate,
            int? dayRange)
        {
            DayRange = dayRange;
            MinDate = minDate;
            MaxDate = maxDate;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public int? DayRange { get; }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}