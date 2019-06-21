namespace EB.Features.Site.Login.Requests
{
    using MediatR;

    public class LoginQuery : IRequest<LoginViewModel>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string HideLanguage { get; set; }

        public string ReturnUrl { get; set; }
    }
}
