namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;

    public class AbnormalWinLossOfHeadlineCriteria : HouseWinLossBaseCriteria
    {
        private const int AbnormalReportTypeInDB = 2;

        public AbnormalWinLossOfHeadlineCriteria(HouseWinLossParam param) : base(param)
        {
            ReportMode = param.IsHistoricReport ? Constants.ReportMode.History : Constants.ReportMode.Current;
            MoveBetsDay = param.MoveBetsDay;
        }

        public DateTime MoveBetsDay { get; }

        public int ReportMode { get; }

        public int ReportType { get; } = AbnormalReportTypeInDB;

        public override string GetSettingKey()
            => IsCountUser ? "CS_EB_Rpt_Cancelled_Headline_CustCount" : "CS_EB_Rpt_Cancelled_Headline";
    }
}
