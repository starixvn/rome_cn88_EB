using System;
using EB.Features.Reports.Params;
using Fanex.Data.Repository;

namespace EB.Features.Reports.Criteria
{
    public class MatchWinLossCriteria : CriteriaBase
    {
        public MatchWinLossCriteria(MatchWinLossParam param)
        {
            WinLostDate = param.WinLossDate;
            SportTypeGroup = param.SportTypeGroup;
            BetTypeGroup = param.BetTypeGroup;
            IsLive = param.IsLive;
            IsExcludedTest = param.IsExcludedTest;
            MoveBetsDay = param.MoveBetsDay;
        }

        public DateTime WinLostDate { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int IsLive { get; }

        public bool IsExcludedTest { get; }

        public DateTime MoveBetsDay { get; }

        public override bool IsValid()
            => true;

        public override string GetSettingKey()
            => "CS_Rpt_MatchWinLoss_Headline";
    }
}
