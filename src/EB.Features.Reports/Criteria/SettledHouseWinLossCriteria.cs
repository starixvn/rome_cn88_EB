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
            SportTypeGroups = param.SportTypeGroups;
            BetTypeGroup = param.BetTypeGroup;
            IsLive = param.IsLive;
            IsExcludedTest = param.IsExcludedTest;
            ArchiveDate = param.ArchiveDate;
            MoveBetsDay = param.MoveBetsDay;
            ReportMode = param.IsHistoricReport ? Constants.ReportMode.History : Constants.ReportMode.Current;
        }

        public int ReportMode { get; }

        public DateTime ArchiveDate { get; }

        public DateTime MoveBetsDay { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string SportTypeGroups { get; }

        public int BetTypeGroup { get; }

        public int IsLive { get; }

        public bool IsExcludedTest { get; }

        public override bool IsValid()
            => BetTypeGroup >= 0 && IsLive >= 0 && FromDate <= ToDate;

        public override string GetSettingKey()
            => "CS_Rpt_Winloss_Headline";
    }
}
