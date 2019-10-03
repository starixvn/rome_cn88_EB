namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using Fanex.Data.Repository;

    public class MatchWinLossCriteria : CriteriaBase
    {
        public MatchWinLossCriteria(MatchWinLossParam param)
        {
            WinLostDate = param.WinLossDate;
            SportTypeGroups = param.SportTypeGroups;
            BetTypeGroup = param.BetTypeGroup;
            IsLive = param.IsLive;
            IsExcludedTest = param.IsExcludedTest;
            MoveBetsDay = param.MoveBetsDay;
        }

        public DateTime WinLostDate { get; }

        public string SportTypeGroups { get; }

        public int BetTypeGroup { get; }

        public int IsLive { get; }

        public bool IsExcludedTest { get; }

        public DateTime MoveBetsDay { get; }

        public override bool IsValid()
            => BetTypeGroup >= 0 && IsLive >= 0;

        public override string GetSettingKey()
            => "CS_EB_Rpt_MatchWinLoss_Headline";
    }
}
