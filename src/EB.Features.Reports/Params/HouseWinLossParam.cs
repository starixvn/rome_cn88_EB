using System;

namespace EB.Features.Reports.Params
{
    public class HouseWinLossParam
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SportTypeGroup { get; set; }

        public int BetTypeGroup { get; set; }

        public int IsLive { get; set; }

        public bool IsExcludedTest { get; set; }
    }
}
