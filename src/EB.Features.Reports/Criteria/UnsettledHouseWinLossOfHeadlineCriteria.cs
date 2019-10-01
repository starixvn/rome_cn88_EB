namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using Fanex.Data.Repository;

    public class UnsettledHouseWinLossOfHeadlineCriteria : CriteriaBase
    {
        public UnsettledHouseWinLossOfHeadlineCriteria(HouseWinLossParam param)
        {
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            SportTypeGroups = param.SportTypeGroups;
            BetTypeGroup = param.BetTypeGroup;
            IsLive = param.IsLive;
            IsExcludedTest = param.IsExcludedTest;            
        }
        
        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string SportTypeGroups { get; }

        public int BetTypeGroup { get; }

        public int IsLive { get; }

        public bool IsExcludedTest { get; }

        public override bool IsValid()
            => BetTypeGroup >= 0 && IsLive >= 0 && FromDate <= ToDate;

        public override string GetSettingKey()
            => "CS_Rpt_EBOutstanding_Headline";
    }
}
