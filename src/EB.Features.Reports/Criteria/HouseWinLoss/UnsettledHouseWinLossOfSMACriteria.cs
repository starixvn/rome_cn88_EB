namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Model;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UnsettledHouseWinLossOfSMACriteria : HouseWinLossBaseCriteria
    {
        public UnsettledHouseWinLossOfSMACriteria(HouseWinLossParam param) : base(param)
        {
            CustId = param.CustId;
            Role = param.Role;
        }

        [SpParam(Ignored = true)]
        public UserLevel Role { get; }

        public int CustId { get; }

        public override bool IsValid() => base.IsValid() && CustId >= 0;

        public override string GetSettingKey()
            => Role.IsSuperLevel() ?
            "CS_Rpt_EBOutstanding_Super" :
            Role.IsMasterLevel() ?
            "CS_Rpt_EBOutstanding_Master" :
            Role.IsAgentLevel() ?
            "CS_Rpt_EBOutstanding_Agent" :
            "CS_Rpt_EBOutstanding_Member";
    }
}
