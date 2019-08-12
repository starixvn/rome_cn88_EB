namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Model;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UnsettledHouseWinLossOfSMACriteria : CriteriaBase
    {
        public UnsettledHouseWinLossOfSMACriteria(HouseWinLossParam param)
        {
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
            "CS_Rpt_EBOutstanding_Super" :
            Role.IsMasterLevel()?
            "CS_Rpt_EBOutstanding_Master" :
            Role.IsAgentLevel()?
            "CS_Rpt_EBOutstanding_Agent" :
            "CS_Rpt_EBOutstanding_Member";
    }
}
