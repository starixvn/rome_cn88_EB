namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Model;
    using Fanex.Data;

    public class SettledCountSMAHouseWLCriteria : HouseWinLossBaseCriteria
    {

        public SettledCountSMAHouseWLCriteria(HouseWinLossParam param) : base(param)
        {
            Role = param.Role;
            CustId = param.CustId;
            HeadlineId = param.HeadlineId;
            ReportMode = param.IsHistoricReport ? Constants.ReportMode.History : Constants.ReportMode.Current;
            MoveBetsDay = param.MoveBetsDay;
        }

        public int ReportMode { get; }

        public DateTime MoveBetsDay { get; }

        [SpParam(Ignored = true)]
        public UserLevel Role { get; }

        public int HeadlineId { get; }

        public int CustId { get; }

        public override bool IsValid() => base.IsValid() && HeadlineId >= 0 && CustId >= 0;

        public override string GetSettingKey()
            => Role.IsSuperLevel() ?
            "CS_EB_Rpt_Winloss_Super_CustCount" :
             Role.IsMasterLevel() ?
            "CS_EB_Rpt_Winloss_Master_CustCount" :
            Role.IsAgentLevel() ?
            "CS_EB_Rpt_Winloss_Agent_CustCount" :
            "CS_EB_Rpt_Winloss_Member_CustCount";
    }
}
