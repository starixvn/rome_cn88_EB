namespace EB.Features.Shared.Utilities
{
    using System;

    public static class SystemTime
    {
        [ThreadStatic]
        private static DateTime? today;
        private static DateTime? yesterday;
        private static DateTime? minDate;

        public static DateTime MinDate
        {
            get { return minDate ?? new DateTime(2019, 1, 1); }
            internal set { minDate = value; }
        }

        public static DateTime Today
        {
            get { return today ?? DateTime.Today; }
            internal set { today = value; }
        }

        public static DateTime Yesterday
        {
            get { return yesterday ?? DateTime.Today.AddDays(-1); }
            internal set { yesterday = value; }
        }
    }
}
