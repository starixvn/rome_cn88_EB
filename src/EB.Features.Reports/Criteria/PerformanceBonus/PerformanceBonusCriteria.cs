namespace EB.Features.Reports.Criteria
{
    using System;
    using EB.Features.Reports.Params;
    using Fanex.Data.Repository;

    public class PerformanceBonusCriteria : CriteriaBase
    {
        public PerformanceBonusCriteria(PerformanceBonusParam param)
        {
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            Currency = param.CurrencyId;
            HeadlineStock = param.HeadlineStock;
            SuperUsernames = param.UserNames;
            Conjunction = param.ConjunctionId;
        }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int Currency { get; }

        public decimal HeadlineStock { get; }

        public string SuperUsernames { get; }

        public int Conjunction { get; }

        public override bool IsValid()
            => FromDate <= ToDate && Currency > 0 && Conjunction > 0;

        public override string GetSettingKey()
            => "CS_EB_Rpt_PerformanceBonus";
    }
}
