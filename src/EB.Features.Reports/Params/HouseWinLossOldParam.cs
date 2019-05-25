using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Reports.Params
{
    public class HouseWinLossOldParam : HouseWinLossParam
    {
        public DateTime ArchiveDate { get; set; }

        public DateTime MoveBetsDay { get; set; }
    }
}
