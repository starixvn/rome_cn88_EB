namespace EB.Features.Reports.Params
{
    using System;
    using EB.Features.Shared.Model;

    public class HouseWinLossParam
    {
        public HouseWinLossParam()
        {
            Role = UserLevel.Headline;
        }

        public UserLevel Role { get; set; }

        public int HeadlineId { get; set; }

        public int CustId { get; set; }

        public int Viewer { get; set; }

        public bool IsHistoricReport { get; set; }

        public int TicketStatusId { get; set; }

        public DateTime Date { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SportTypeGroups { get; set; }

        public int BetTypeGroup { get; set; }

        public int IsLive { get; set; }

        public bool IsExcludedTest { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public bool IsCountUser { get; set; }
    }
}
