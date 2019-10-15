namespace EB.Features.Main.Services
{
    using System;
    using System.Collections.Generic;
    using EB.Features.Main.Criteria.InternalLogin;
    using EB.Features.Main.DataAccesses;
    using EB.Features.Main.Params;

    public interface InternalLoginService
    {
        string GetInternalLogInLink(InternalLoginParam param);
    }

    public class InternalLoginServiceImpl : InternalLoginService
    {
        private readonly InternalLoginDataAccess dataAccess;

        public InternalLoginServiceImpl(InternalLoginDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public string GetInternalLogInLink(InternalLoginParam param)
        {
            const bool isHideTopMenu = true;
            var time = dataAccess.GetDBTime(new GetDBTimeCriteria()).Ticks;
            var queryParam = string.Join("&", new List<object>
            {
                param.CustName,
                param.AccId,
                time,
                string.Empty,
                param.AfterLogInSite,
                param.AfterLogInQuery.Replace("&", "|"),
                string.Empty,
                param.Language,
                isHideTopMenu
            });

            var encryptedParams = param.EncryptParamFunc(queryParam, 1024, param.KeyPath);

            var result = string.Format(
                "{0}?{1}params={2}&isVpn={3}",
                param.LogInLink,
                param.InternalLogInUMToken,
                encryptedParams,
                IsLocalOrVpnRequest(param.IP) ? 1 : 0);

            return result;
        }

        private static bool IsLocalOrVpnRequest(string ip)
            => ip.Split('.').Length == 4 &&
                (ip.StartsWith("10.", StringComparison.OrdinalIgnoreCase) ||
                ip.StartsWith("172.16.", StringComparison.OrdinalIgnoreCase) ||
                ip.StartsWith("192.168.", StringComparison.OrdinalIgnoreCase));
    }
}
