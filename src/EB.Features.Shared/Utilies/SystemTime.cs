using System;

namespace EB.Features.Shared.Utilies
{
    public static class SystemTime
    {
        [ThreadStatic]
        private static DateTime? today;

        public static DateTime Today
        {
            get { return today ?? DateTime.Today; }
            internal set { today = value; }
        }
    }
}
