using System;

namespace EB.Features.Reports.Model
{
    public class PerformanceBonusModel
    {

        public PerformanceBonusModel()
        {
        }

        public int CustId { get; }

        public string UserName { get; }

        public int HeadlineId { get; }

        public string HeadlineUserName { get; }

        public decimal HeadlineStock { get; internal set; }

        public decimal PerformanceBonus { get; set; }

        public decimal BonusRate { get; set; }

        public decimal PerformanceBonus1035 { get; set; }

        public decimal RoundDown { get; set; }

        public static PerformanceBonusModel operator +(PerformanceBonusModel match1, PerformanceBonusModel match2)
            => new PerformanceBonusModel
            {
                PerformanceBonus = match1.PerformanceBonus + match2.PerformanceBonus,
                PerformanceBonus1035 = match1.PerformanceBonus1035 + match2.PerformanceBonus1035,
                RoundDown = match1.RoundDown + match2.RoundDown,
                HeadlineStock = match1.HeadlineStock + match2.HeadlineStock
            };
    }
}
