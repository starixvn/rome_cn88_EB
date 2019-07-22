namespace EB.Features.AccountManagement.Criteria.SubAccount
{
    using Fanex.Data.Repository;

    internal class GetSuperSubAccountCriteria : CriteriaBase
    {
        public GetSuperSubAccountCriteria(string username)
        {
            Username = username;
        }

        public string Username { get; }

        public override string GetSettingKey()
            => "CS_CustSubSelInfoSuper";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username);
    }
}
