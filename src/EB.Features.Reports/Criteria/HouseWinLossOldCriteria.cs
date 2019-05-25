using EB.Features.Reports.Params;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
