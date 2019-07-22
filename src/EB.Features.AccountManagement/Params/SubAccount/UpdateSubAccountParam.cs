namespace EB.Features.AccountManagement.Params.SubAccount
{
    public class UpdateSubAccountParam
    {
        public UpdateSubAccountParam(
            int custId,
            int subAccountId,
            string username,
            string pageAccess,
            string languageKey,
            string ip,
            int actorId)
        {
            CustId = custId;
            SubAccId = subAccountId;
            Username = username;
            PageAccess = pageAccess;
            LanguageKey = languageKey;
            IP = ip;
            ActorId = actorId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Username { get; }

        public string Name { get; set; }

        public string PageAccess { get; set; }

        public string LanguageKey { get; }

        public string IP { get; }

        public int ActorId { get; }
    }
}
