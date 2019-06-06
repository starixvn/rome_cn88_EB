namespace EB.Features.Shared.Model
{
    using System;

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
