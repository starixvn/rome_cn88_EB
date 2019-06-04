namespace EB.Features.Reports.Params
{
    using System;

    public class HouseWinLossOldParam : HouseWinLossParam
    {
        public DateTime ArchiveDate { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}
