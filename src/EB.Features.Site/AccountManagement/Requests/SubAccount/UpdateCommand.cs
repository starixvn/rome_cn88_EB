namespace EB.Features.Site.AccountManagement.Requests.SubAccount
{
    using Fanex.NAP.Core.Web.Session;
    using Features.Shared.Model;
    using Features.Site.Shared;
    using MediatR;

    public class UpdateCommand : BaseQuery, IRequest<Result>
    {
        public UpdateCommand(ICurrentSession currentSession)
            : base(currentSession)
        {
            ActorId = currentSession.UserId;
        }

        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string PageAccess { get; set; }

        public string Language { get; set; }

        public string Username { get; set; }

        public int ActorId { get; }

        public override bool IsValid()
            => CustId > 0 && SubAccId > 0 && ActorId > 0 && !string.IsNullOrWhiteSpace(Username);
    }
}