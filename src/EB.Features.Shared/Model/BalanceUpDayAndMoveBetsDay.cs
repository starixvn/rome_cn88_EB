using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EB.Features.Shared.Model
{
    public class BalanceUpDayAndMoveBetsDay
    {
        public BalanceUpDayAndMoveBetsDay()
        {
        }

        public BalanceUpDayAndMoveBetsDay(DateTime balanceUpDay, DateTime moveBetsDay)
        {
            BalanceUpDay = balanceUpDay;
            MoveBetsDay = moveBetsDay;
        }

        public DateTime BalanceUpDay { get; }

        public DateTime MoveBetsDay { get; }
    }
}
