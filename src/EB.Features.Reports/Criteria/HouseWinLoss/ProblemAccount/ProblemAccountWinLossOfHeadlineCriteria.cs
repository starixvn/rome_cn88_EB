namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;

    public class ProblemAccountWinLossOfHeadlineCriteria : HouseWinLossBaseCriteria
    {
        public ProblemAccountWinLossOfHeadlineCriteria(HouseWinLossParam param) : base(param)
        {
            ReportMode = param.IsHistoricReport ? Constants.ReportMode.History : Constants.ReportMode.Current;
            MoveBetsDay = param.MoveBetsDay;
        }

        public DateTime MoveBetsDay { get; }

        public int ReportMode { get; }

        public override string GetSettingKey()
            => IsCountUser ? "CS_EB_Rpt_Winloss_ProblemAccount_Headline_CustCount" : "CS_EB_Rpt_Winloss_ProblemAccount_Headline";
    }
}
