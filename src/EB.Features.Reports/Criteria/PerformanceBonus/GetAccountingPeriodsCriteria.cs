namespace EB.Features.Reports.Criteria.PerformanceBonus
{
    using Fanex.Data.Repository;

    public class GetAccountingPeriodsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_EB_Period_Sel";

        public override bool IsValid() => true;
    }
}
