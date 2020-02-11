namespace EB.Features.Reports.Criteria
{
    using System.Collections;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Model;
    using Fanex.Data;

    public class UnsettledHouseWinLossOfSMACriteria : HouseWinLossBaseCriteria
    {
        public UnsettledHouseWinLossOfSMACriteria(HouseWinLossParam param) : base(param)
        {
            HeadlineId = param.HeadlineId;
            CustId = param.CustId;
            Role = param.Role;
        }

        public int HeadlineId { get; }

        [SpParam(Ignored = true)]
        public UserLevel Role { get; }

        public int CustId { get; }

        public override bool IsValid() => base.IsValid() && CustId >= 0 && HeadlineId >= 0;

        public override string GetSettingKey()
        {
            var key = Role.IsSuperLevel() ?
            "CS_EB_Rpt_Outstanding_Super" :
            Role.IsMasterLevel() ?
            "CS_EB_Rpt_Outstanding_Master" :
            Role.IsAgentLevel() ?
            "CS_EB_Rpt_Outstanding_Agent" :
            "CS_EB_Rpt_Outstanding_Member";

            return IsCountUser ? $"{key}_CustCount" : key;
        }
    }
}
