namespace EB.Features.Site.AccountManagement.Requests.SubAccount
{
    using EB.Features.Site.Shared.Models;
    using Features.AccountManagement.Models.SubAccount;
    using MediatR;
    using Shared;

    public class SearchQuery : BaseQuery, IRequest<SubAccountItem>
    {
        public SearchQuery(UserSession currentSession)
            : base(currentSession)
        {
        }

        public string Name { get; set; }
    }
}