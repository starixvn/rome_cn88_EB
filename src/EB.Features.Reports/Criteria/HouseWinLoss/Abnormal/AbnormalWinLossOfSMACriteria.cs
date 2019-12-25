namespace EB.Features.Reports.Criteria
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using EB.Features.Reports.Params;
    using EB.Features.Shared.Constants;
    using EB.Features.Shared.Model;
    using Fanex.Data;

    public class AbnormalWinLossOfSMACriteria : HouseWinLossBaseCriteria
    {
        private const int AbnormalReportTypeInDB = 2;

        public AbnormalWinLossOfSMACriteria(HouseWinLossParam param) : base(param)
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

        public int ReportType { get; } = AbnormalReportTypeInDB;

        public override bool IsValid() => base.IsValid() && CustId >= 0;

        public override string GetSettingKey()
        {
            var key = Role.IsSuperLevel() ?
            "CS_EB_Rpt_Cancelled_Super" :
             Role.IsMasterLevel() ?
            "CS_EB_Rpt_Cancelled_Master" :
            Role.IsAgentLevel() ?
            "CS_EB_Rpt_Cancelled_Agent" :
            "CS_EB_Rpt_Cancelled_Member";

            return IsCountUser ? $"{key}_CustCount" : key;
        }
    }
}
