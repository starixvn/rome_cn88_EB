namespace EB.Features.Site.Login.Requests
{
    using System;
    using System.Web;
    using EB.Features.Shared.Configuration;
    using EB.Features.Site.Shared;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;
    using RSACryptoPad;

    public class InternalLoginQuery : BaseQuery, IRequest<string>
    {
        public InternalLoginQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
            AccId = currentSession.UserId;

            KeyPath = HttpContext.Current.Request.PhysicalApplicationPath + "/IbcKeys/ibcpkey.snk";
            EncryptParamFunc = (str, size, path) => new EncryptionThread().EncryptString(str, size, path);
            InternalLogInUMToken = AppSettings.InternalLogInUMToken;
            IP = HttpContext.Current.Request.Headers["Host"];
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte RoleId { get; set; }

        public int AccId { get; }

        public string KeyPath { get; }

        public Func<string, int, string, string> EncryptParamFunc { get; }

        public string InternalLogInUMToken { get; }

        public string IP { get; }

        public string InternalLogInURL
            => RoleId == 0 ? AppSettings.InternalLogInURLHeadline : AppSettings.InternalLogInURL;
    }
}
