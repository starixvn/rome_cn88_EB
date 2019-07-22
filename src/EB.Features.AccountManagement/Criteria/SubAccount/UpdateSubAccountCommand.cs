namespace EB.Features.AccountManagement.Criteria.SubAccount
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params.SubAccount;

    public class UpdateSubAccountCommand : NonQueryCommand
    {
        public UpdateSubAccountCommand(UpdateSubAccountParam param)
        {
            CustId = param.CustId;
            SubAccId = param.SubAccId;
            Name = param.Name ?? string.Empty;
            PageAccess = param.PageAccess;
            LanguageKey = param.LanguageKey;
            LoginIP = param.IP;
            UserId = param.ActorId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        [SpParam(SqlParamName = "firstname")]
        public string Name { get; }

        public string PageAccess { get; }

        [SpParam(SqlParamName = "Language")]
        public string LanguageKey { get; }

        public string LoginIP { get; }

        public int UserId { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "CS_CustSubUpd";

        public override bool IsValid()
            => CustId > 0 && SubAccId > 0 && UserId > 0;
    }
}
