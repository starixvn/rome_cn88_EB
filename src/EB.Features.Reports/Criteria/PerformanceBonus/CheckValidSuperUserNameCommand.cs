namespace EB.Features.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class CheckValidSuperUserNameCommand : NonQueryCommand
    {
        public CheckValidSuperUserNameCommand(string userNames)
        {
            Usernames = userNames;
        }

        public string Usernames { get; }

        public string InvalidUsernames { get; set; }

        public override bool IsValid()
            => !string.IsNullOrEmpty(Usernames);

        public override string GetSettingKey()
            => "CS_Customer_CheckValidUsername";
    }
}
