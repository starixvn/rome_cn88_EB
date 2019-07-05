namespace EB.Features.Site.Login.Requests
{
    using System.Web;
    using Fanex.NAP.Core.Configuration;
    using MediatR;

    public class LoginQuery : IRequest<LoginViewModel>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string HideLanguage { get; set; }

        public string ReturnUrl { get; set; }

        public string SessionId { get; set; }

        public int SiteId { get; set; }
    }
}
