namespace EB.Features.Site.Login.Requests
{
    using System;
    using System.Web;
    using EB.Features.Shared.Configuration;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Models;
    using MediatR;
    using RSACryptoPad;

    public class InternalLoginQuery : BaseQuery, IRequest<string>
    {
        public InternalLoginQuery(UserSession currentSession)
            : base(currentSession)
        {
        }

        public int CustId { get; set; }

        public string Languagekey { get; set; }

        public string CustName { get; set; }

        public byte RoleId { get; set; }

        public string AfterLogInSite { get; set; }

        public string AfterLogInQuery { get; set; }

        public int AccId
            => CurrentSession.UserId;

        public string KeyPath
            => HttpContext.Current.Request.PhysicalApplicationPath + "/IbcKeys/ibcpkey.snk";

        public Func<string, int, string, string> EncryptParamFunc
            => (str, size, path) => new EncryptionThread().EncryptString(str, size, path);

        public string InternalLogInUMToken
            => HttpContext.Current.Session["AlphaUMToken"] as string;

        public string IP => CurrentSession.Ip;

        public string InternalLogInURL
            => RoleId == 0 ? AppSettings.InternalLogInURLHeadline : AppSettings.InternalLogInURL;
    }
}
