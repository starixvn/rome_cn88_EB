namespace EB.Features.Main.Criteria.InternalLogin
{
    using Fanex.Data.Repository;

    public class CheckIPOfficeCommand : NonQueryCommand
    {
        public CheckIPOfficeCommand(string userName, string loginIp)
        {
            UserName = userName;
            LoginIp = loginIp;
        }

        public string UserName { get; set; }

        public string LoginIp { get; set; }

        public bool IsAccess { get; set; }

        public override string GetSettingKey() => "CS_LoginAdminCheckGrantAccess";

        public override bool IsValid() => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(LoginIp);
    }
}
