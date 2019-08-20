namespace EB.Features.Reports.Params
{
    using System;

    public class MatchWinLossParam
    {
        public DateTime WinLossDate { get; set; }

        public string SportTypeGroups { get; set; }

        public int BetTypeGroup { get; set; }

        public int IsLive { get; set; }

        public bool IsExcludedTest { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}
