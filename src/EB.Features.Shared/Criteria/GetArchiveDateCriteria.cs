using Fanex.Data.Repository;

namespace EB.Features.Shared.Criteria
{
    public class GetArchiveDateCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_SelArchiveDate";

        public override bool IsValid() => true;
    }
}
