namespace EB.Features.Site.Main.Requests
{
    using System.Collections.Generic;
    using EB.Features.Main.Model;
    using EB.Features.Site.Shared;
    using Fanex.NAP.Core.Web.Session;
    using MediatR;

    public class MenuBarQuery : BaseQuery, IRequest<IEnumerable<MenuItem>>
    {
        public MenuBarQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }
    }
}