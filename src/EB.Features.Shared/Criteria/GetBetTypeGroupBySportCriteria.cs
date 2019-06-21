namespace EB.Features.Shared.Criteria
{
    using Fanex.Data.Repository;

    public class GetBetTypeGroupBySportCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_BetType_Sel";

        public override bool IsValid() => true;
    }
}
