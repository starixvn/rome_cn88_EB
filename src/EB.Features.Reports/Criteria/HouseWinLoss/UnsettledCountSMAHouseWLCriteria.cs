namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Model;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UnsettledCountSMAHouseWLCriteria : CriteriaBase
    {
        public UnsettledCountSMAHouseWLCriteria(HouseWinLossParam param)
        {
            HeadlineId = param.HeadlineId;
            CustId = param.CustId;
            Role = param.Role;
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            SportTypeGroups = param.SportTypeGroups;
            BetTypeGroup = param.BetTypeGroup;
            IsLive = param.IsLive;
            IsExcludedTest = param.IsExcludedTest;            
        }

        [SpParam(Ignored = true)]
        public UserLevel Role { get; }

        public int HeadlineId { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string SportTypeGroups { get; }

        public int BetTypeGroup { get; }

        public int IsLive { get; }

        public bool IsExcludedTest { get; }

        public override bool IsValid()
            => BetTypeGroup >= 0 && IsLive >= 0 && FromDate <= ToDate;

        public override string GetSettingKey()
            => Role.IsSuperLevel() ?
            "CS_EB_Rpt_Outstanding_Super_CustCount" :
            Role.IsMasterLevel()?
            "CS_EB_Rpt_Outstanding_Master_CustCount" :
            Role.IsAgentLevel()?
            "CS_EB_Rpt_Outstanding_Agent_CustCount" :
            "CS_EB_Rpt_Outstanding_Member_CustCount";
    }
}
