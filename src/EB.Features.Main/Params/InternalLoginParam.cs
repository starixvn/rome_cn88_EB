namespace EB.Features.Main.Params
{
    using EB.Features.Shared.Utilities;
    using System;

    public class InternalLoginParam
    {
        public InternalLoginParam(
            int custId,
            string custName,
            int accId,
            string languageKey,
            string logInLink,
            string internalLogInUMToken,
            string ip,
            bool isHideTopMenu,
            string afterLogInSite,
            string afterLogInQuery,
            string keyPath,
            Func<string, int, string, string> encryptParamFunc)
        {
            CustId = custId;
            CustName = custName;
            Language = languageKey;
            AccId = accId;
            LogInLink = logInLink.StartsWith("http") ? logInLink : "http://" + logInLink;
            InternalLogInUMToken = string.IsNullOrWhiteSpace(internalLogInUMToken) ? string.Empty : $"UMToken={internalLogInUMToken}&";
            IP = ip;
            IsHideTopMenu = isHideTopMenu;
            AfterLogInSite = afterLogInSite;
            AfterLogInQuery = afterLogInQuery;

            KeyPath = keyPath;
            EncryptParamFunc = encryptParamFunc;
        }

        public bool IsHideTopMenu { get; }

        public int CustId { get; }

        public string CustName { get; }

        public int AccId { get; }

        public string Language { get; }

        public string LogInLink { get; }

        public string InternalLogInUMToken { get; }

        public string IP { get; }

        public string AfterLogInSite { get; }

        public string AfterLogInQuery { get; }

        public string KeyPath { get; }

        public Func<string, int, string, string> EncryptParamFunc { get; }
    }
}
