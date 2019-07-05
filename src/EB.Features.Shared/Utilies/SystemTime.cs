namespace EB.Features.Shared.Utilies
{
    using System;

    public static class SystemTime
    {
        [ThreadStatic]
        private static DateTime? today;
        private static DateTime? yesterday;

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
