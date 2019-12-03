namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;

    public class SettledCountHeadlineHouseWLCriteria : HouseWinLossBaseCriteria
    {

        public SettledCountHeadlineHouseWLCriteria(HouseWinLossParam param) : base(param)
        {
            ReportMode = param.IsHistoricReport ? Constants.ReportMode.History : Constants.ReportMode.Current;
            MoveBetsDay = param.MoveBetsDay;
        }

        public DateTime MoveBetsDay { get; }

        public int ReportMode { get; }

        public override string GetSettingKey()
            => "CS_EB_Rpt_Winloss_Headline_CustCount";
    }
}
