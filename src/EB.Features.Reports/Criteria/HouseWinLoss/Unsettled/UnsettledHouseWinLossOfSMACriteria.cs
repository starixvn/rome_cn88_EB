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

        [SpParam(Ignored = true)]
        public UserLevel Role { get; }

        public int HeadlineId { get; }

        public int CustId { get; }

        public override bool IsValid() => base.IsValid() && CustId >= 0 && HeadlineId >= 0;

        public override string GetSettingKey()
        {
            var key = Role.IsSuperLevel() ?
            "CS_Rpt_EBOutstanding_Super" :
            Role.IsMasterLevel() ?
            "CS_Rpt_EBOutstanding_Master" :
            Role.IsAgentLevel() ?
            "CS_Rpt_EBOutstanding_Agent" :
            "CS_Rpt_EBOutstanding_Member";

            return IsCountUser ? $"{key}_CustCount" : key;
        }

        public override Hashtable GetSpParams()
        {
            var spParams = base.GetSpParams();
            if (!IsCountUser)
            {
                spParams.Remove("HeadlineId");
            }

            return spParams;
        }
    }
}
