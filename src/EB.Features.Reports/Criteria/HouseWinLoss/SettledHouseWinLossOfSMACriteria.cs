namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Model;
    using Fanex.Data;

    public class SettledHouseWinLossOfSMACriteria : HouseWinLossBaseCriteria
    {
        public SettledHouseWinLossOfSMACriteria(HouseWinLossParam param) : base(param)
        {
            HeadlineId = param.HeadlineId;
            Role = param.Role;
            CustId = param.CustId;
            MoveBetsDay = param.MoveBetsDay;
            ReportMode = param.IsHistoricReport ? Constants.ReportMode.History : Constants.ReportMode.Current;
        }

        public int HeadlineId { get; }

        [SpParam(Ignored = true)]
        public UserLevel Role { get; }

        public int CustId { get; }

        public int ReportMode { get; }

        public DateTime MoveBetsDay { get; }

        public override bool IsValid() => base.IsValid() && CustId >= 0;

        public override string GetSettingKey()
            => Role.IsSuperLevel() ?
            "CS_EB_Rpt_Winloss_Super" :
             Role.IsMasterLevel() ?
            "CS_EB_Rpt_Winloss_Master" :
            Role.IsAgentLevel() ?
            "CS_EB_Rpt_Winloss_Agent" :
            "CS_EB_Rpt_Winloss_Member";
    }
}
