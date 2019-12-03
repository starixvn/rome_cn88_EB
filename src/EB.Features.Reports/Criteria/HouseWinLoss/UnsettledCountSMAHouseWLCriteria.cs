namespace EB.Features.Reports.Criteria
{
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Model;
    using Fanex.Data;

    public class UnsettledCountSMAHouseWLCriteria : HouseWinLossBaseCriteria
    {
        public UnsettledCountSMAHouseWLCriteria(HouseWinLossParam param) : base(param)
        {
            HeadlineId = param.HeadlineId;
            CustId = param.CustId;
            Role = param.Role;
        }

        [SpParam(Ignored = true)]
        public UserLevel Role { get; }

        public int HeadlineId { get; }

        public int CustId { get; }

        public override bool IsValid() => base.IsValid() && HeadlineId >= 0 && CustId >= 0;

        public override string GetSettingKey()
            => Role.IsSuperLevel() ?
            "CS_EB_Rpt_Outstanding_Super_CustCount" :
            Role.IsMasterLevel() ?
            "CS_EB_Rpt_Outstanding_Master_CustCount" :
            Role.IsAgentLevel() ?
            "CS_EB_Rpt_Outstanding_Agent_CustCount" :
            "CS_EB_Rpt_Outstanding_Member_CustCount";
    }
}
