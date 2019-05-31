using System;
using EB.Features.Reports.Params;

namespace EB.Features.Reports.Criteria
{
    public class HouseWinLossOldCriteria : HouseWinLossCriteria
    {
        public HouseWinLossOldCriteria(HouseWinLossOldParam param)
            : base(param)
        {
            MoveBetsDay = param.MoveBetsDay;
            ArchiveDate = param.ArchiveDate;
        }

        public DateTime MoveBetsDay { get; }

        public DateTime ArchiveDate { get; }

        public override string GetSettingKey()
            => "CS_Rpt_Winloss_Headline";
    }
}
