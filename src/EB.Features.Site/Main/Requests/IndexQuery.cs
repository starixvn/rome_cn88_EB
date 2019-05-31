using EB.Features.Site.Main.ViewModel;
using EB.Features.Site.Shared;
using Fanex.NAP.Core.Web.Session;
using MediatR;

namespace EB.Features.Site.Main.Requests
{
    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        public IndexQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }
    }
}