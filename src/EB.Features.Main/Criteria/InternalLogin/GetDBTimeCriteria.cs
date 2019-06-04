namespace EB.Features.Main.Criteria.InternalLogin
{
    using Fanex.Data.Repository;

    public class GetDBTimeCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_LoginAdminVerifyTicket";

        public override bool IsValid() => true;
    }
}
