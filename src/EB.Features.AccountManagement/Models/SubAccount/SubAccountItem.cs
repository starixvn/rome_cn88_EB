namespace EB.Features.AccountManagement.Models.SubAccount
{
    using Sunplus.Agent.Model.Entities;

    public class SubAccountItem
    {
        public SubAccountItem()
        {
        }

        public SubAccountItem(
            string smaUsername,
            string smaNickname,
            int custId,
            int subAccId,
            string userName,
            string nickName,
            string firstName,
            string pageAccess)
        {
            SMAUsername = smaUsername;
            SMANickname = smaNickname;
            CustId = custId;
            SubAccId = subAccId;
            UserName = userName;
            NickName = nickName;
            FirstName = firstName;
            PageAccess = pageAccess;
        }

        public string SMAUsername { get; }

        public string SMANickname { get; }

        public int CustId { get; }

        public int SubAccId { get; set; }

        public string UserName { get; }

        public string NickName { get; }

        public string FirstName { get; }

        public string PageAccess { get; }

        public PageAccess Permissions => PageAccess;
    }
}
