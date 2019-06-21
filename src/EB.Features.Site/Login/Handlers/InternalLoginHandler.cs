namespace EB.Features.Site.Login.Handlers
{
    using EB.Features.Main.Params;
    using EB.Features.Main.Services;
    using EB.Features.Site.Login.Requests;
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
                message.InternalLogInURL,
                message.InternalLogInUMToken,
                message.IP,
                message.AfterLogInSite,
                message.AfterLogInQuery,
                message.KeyPath,
                message.EncryptParamFunc);

            return service.GetInternalLogInLink(param);
        }
    }
}
