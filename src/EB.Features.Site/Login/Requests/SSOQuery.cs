namespace EB.Features.Site.Login.Requests
{
    using MediatR;

    public class SSOQuery : IRequest<LoginViewModel>
    {
        public string UserId { get; set; }

        public string OnUserId { get; set; }

        public string Time { get; set; }

        public string RedirectUrl { get; set; }

        public string SessionId { get; set; }

        public int SiteId { get; set; }

        public string Language { get; } = "en-US";
    }
}