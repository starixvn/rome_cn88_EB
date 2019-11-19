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
            Role = param.Role;
            CustId = param.CustId;
            MoveBetsDay = param.MoveBetsDay;
            ReportMode = param.IsHistoricReport ? Constants.ReportMode.History : Constants.ReportMode.Current;
        }

        [SpParam(Ignored = true)]
        public UserLevel Role { get; }

        public int CustId { get; }

        public int ReportMode { get; }

        public DateTime MoveBetsDay { get; }

        public override bool IsValid() => base.IsValid() && CustId >= 0;

        public override string GetSettingKey()
            => Role.IsSuperLevel() ?
            "CS_Rpt_EBWinloss_Super" :
             Role.IsMasterLevel() ?
            "CS_Rpt_EBWinloss_Master" :
            Role.IsAgentLevel() ?
            "CS_Rpt_EBWinloss_Agent" :
            "CS_Rpt_EBWinloss_Member";
    }
}
