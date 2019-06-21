using System;

namespace EB.Features.Site.Shared.ViewModels.Components
{
    public class DatePickerViewModel
    {
        public DatePickerViewModel(
            DateTime minDate,
            DateTime maxDate,
            DateTime currentDate)
        {
            MinDate = minDate;
            MaxDate = maxDate;
            CurrentDate = currentDate;
        }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime CurrentDate { get; }
    }
}