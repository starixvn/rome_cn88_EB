using Fanex.Data.Repository;

namespace EB.Features.Shared.Criteria
{
    public class GetSportGroupsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_SportType_Sel";

        public override bool IsValid() => true;
    }
}
