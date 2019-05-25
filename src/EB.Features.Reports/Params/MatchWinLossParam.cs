using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Reports.Params
{
    public class MatchWinLossParam
    {
        public DateTime WinLossDate { get; set; }

        public int SportTypeGroup { get; set; }

        public int BetTypeGroup { get; set; }

        public int IsLive { get; set; }

        public bool IsExcludedTest { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}
