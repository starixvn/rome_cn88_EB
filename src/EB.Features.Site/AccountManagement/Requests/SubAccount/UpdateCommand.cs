namespace EB.Features.Site.AccountManagement.Requests.SubAccount
{
    using System.Web;
    using EB.Features.Shared.Utilities;
    using EB.Features.Site.Shared.Models;
    using Features.Shared.Model;
    using Features.Site.Shared;
    using MediatR;

    public class UpdateCommand : BaseQuery, IRequest<Result>
    {
        public UpdateCommand(UserSession currentSession)
            : base(currentSession)
        {
            ActorId = currentSession.UserId;
            IP = currentSession.Ip;
            Language = currentSession.Language;
        }

        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string PageAccess { get; set; }

        public string Language { get; }

        public string Username { get; set; }

        public int ActorId { get; }

        public string IP { get; }

        public override bool IsValid()
            => CustId > 0 && SubAccId > 0 && ActorId > 0 && !string.IsNullOrWhiteSpace(Username);
    }
}