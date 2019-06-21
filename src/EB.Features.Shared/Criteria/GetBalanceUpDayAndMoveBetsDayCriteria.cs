namespace EB.Features.Shared.Criteria
{
    using Fanex.Data.Repository;

    public class GetBalanceUpDayAndMoveBetsDayCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_SelBalanceUpDay";

        public override bool IsValid() => true;
    }
}
