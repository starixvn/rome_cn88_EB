using System;

namespace EB.Features.Reports.Params
{
    public class HouseWinLossOldParam : HouseWinLossParam
    {
        public DateTime ArchiveDate { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}
