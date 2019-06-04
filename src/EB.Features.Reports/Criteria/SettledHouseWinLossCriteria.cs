namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using Fanex.Data.Repository;

    public class SettledHouseWinLossCriteria : CriteriaBase
    {
        public SettledHouseWinLossCriteria(HouseWinLossParam param)
        {
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            SportTypeGroup = param.SportTypeGroup;
            BetTypeGroup = param.BetTypeGroup;
            IsLive = param.IsLive;
            IsExcludedTest = param.IsExcludedTest;
            ArchiveDate = param.ArchiveDate;
            MoveBetsDay = param.MoveBetsDay;
        }

        public DateTime ArchiveDate { get; }

        public DateTime MoveBetsDay { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int IsLive { get; }

        public bool IsExcludedTest { get; }

        public override bool IsValid()
            => true;

        public override string GetSettingKey()
            => "CS_Rpt_Winloss_Headline";
    }
}
