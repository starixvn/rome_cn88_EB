using Fanex.Data.Repository;

namespace EB.Features.Shared.Criteria
{
    public class GetBetTypeGroupBySportCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_BetType_Sel";

        public override bool IsValid() => true;
    }
}
