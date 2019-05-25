using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
