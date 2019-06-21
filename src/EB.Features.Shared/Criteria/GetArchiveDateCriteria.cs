namespace EB.Features.Shared.Criteria
{
    using Fanex.Data.Repository;

    public class GetArchiveDateCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_SelArchiveDate";

        public override bool IsValid() => true;
    }
}
