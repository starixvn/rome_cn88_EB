namespace EB.Features.Site.Login.Handlers
{
    using EB.Features.Main.Params;
    using EB.Features.Main.Services;
    using EB.Features.Site.Login.Requests;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class InternalLoginHandler : IRequestHandler<InternalLoginQuery, string>
    {
        private readonly InternalLoginService service;

        public InternalLoginHandler(InternalLoginService service)
        {
            this.service = service;
        }

        public string Handle(InternalLoginQuery message)
        {
            var param = new InternalLoginParam(
                message.CustId,
                message.CustName,
                message.AccId,
                message.Languagekey,
                message.InternalLogInURL,
                InternalLoginQuery.InternalLogInUMToken,
                message.IP,
                message.AfterLogInSite,
                message.AfterLogInQuery,
                InternalLoginQuery.KeyPath,
                InternalLoginQuery.EncryptParamFunc);

            return service.GetInternalLogInLink(param);
        }
    }
}
