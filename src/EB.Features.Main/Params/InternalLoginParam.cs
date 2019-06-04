namespace EB.Features.Main.Params
{
    using System;

    public class InternalLoginParam
    {
        public InternalLoginParam(
            int custId,
            string custName,
            int accId,
            string logInLink,
            string internalLogInUMToken,
            string ip,
            string keyPath,
            Func<string, int, string, string> encryptParamFunc)
        {
            CustId = custId;
            CustName = custName;
            AccId = accId;
            LogInLink = logInLink.StartsWith("http") ? logInLink : "http://" + logInLink;
            InternalLogInUMToken = string.IsNullOrWhiteSpace(internalLogInUMToken) ? string.Empty : $"UMToken={internalLogInUMToken}&";
            IP = string.IsNullOrWhiteSpace(ip) ? string.Empty : ip;

            KeyPath = keyPath;
            EncryptParamFunc = encryptParamFunc;
        }

        public int CustId { get; }

        public string CustName { get; }

        public int AccId { get; }

        public string LogInLink { get; }

        public string InternalLogInUMToken { get; }

        public string IP { get; }

        public string KeyPath { get; }

        public Func<string, int, string, string> EncryptParamFunc { get; }
    }
}
