namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Model;
    using Fanex.Data;

    public class ProblemAccountWinLossOfSMACriteria : HouseWinLossBaseCriteria
    {
        public ProblemAccountWinLossOfSMACriteria(HouseWinLossParam param) : base(param)
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
        {
            var key = Role.IsSuperLevel() ?
            "CS_EB_Rpt_Winloss_ProblemAccount_Super" :
             Role.IsMasterLevel() ?
            "CS_EB_Rpt_Winloss_ProblemAccount_Master" :
            Role.IsAgentLevel() ?
            "CS_EB_Rpt_Winloss_ProblemAccount_Agent" :
            "CS_EB_Rpt_Winloss_ProblemAccount_Member";

            return IsCountUser ? $"{key}_CustCount" : key;
        }
    }
}
