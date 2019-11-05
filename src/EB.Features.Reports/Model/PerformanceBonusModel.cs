using System;

namespace EB.Features.Reports.Model
{
    public class PerformanceBonusModel
    {

        public PerformanceBonusModel()
        {
        }

        public int CustId { get; }

        public string UserName { get; internal set; }

        public int HeadlineId { get; }

        public string HeadlineUserName { get; internal set; }

        public decimal HeadlineStock { get; internal set; }

        public decimal Dividend { get; set; }

        public decimal DividendRate { get; set; }

        public decimal Bonus { get; set; }

        public decimal BonusRoundDown { get; set; }

        public static PerformanceBonusModel operator +(PerformanceBonusModel match1, PerformanceBonusModel match2)
            => new PerformanceBonusModel
            {
                Dividend = match1.Dividend + match2.Dividend,
                Bonus = match1.Bonus + match2.Bonus,
                BonusRoundDown = match1.BonusRoundDown + match2.BonusRoundDown,
                HeadlineStock = match1.HeadlineStock + match2.HeadlineStock
            };
    }
}
