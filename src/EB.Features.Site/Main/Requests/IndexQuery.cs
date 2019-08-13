namespace EB.Features.Site.Main.Requests
{
    using EB.Features.Site.Main.ViewModel;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        public IndexQuery(UserSession currentSession)
            : base(currentSession)
        {
        }
    }
}