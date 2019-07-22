namespace EB.Features.Site.AccountManagement.Requests.SubAccount
{
    using Fanex.NAP.Core.Web.Session;
    using Features.AccountManagement.Models.SubAccount;
    using MediatR;
    using Shared;

    public class SearchQuery : BaseQuery, IRequest<SubAccountItem>
    {
        public SearchQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }

        public string Name { get; set; }
    }
}