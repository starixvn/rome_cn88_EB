namespace EB.Features.Reports.Params
{
    using System;

    public class HouseWinLossParam
    {
        public bool IsHistoricReport { get; set; }

        public int TicketStatusId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SportTypeGroup { get; set; }

        public int BetTypeGroup { get; set; }

        public int IsLive { get; set; }

        public bool IsExcludedTest { get; set; }

        public DateTime ArchiveDate { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}
