namespace EB.Features.Site.Main.Requests
{
    using EB.Features.Site.Main.ViewModel;
    using EB.Features.Site.Shared;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;

    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        public IndexQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }
    }
}