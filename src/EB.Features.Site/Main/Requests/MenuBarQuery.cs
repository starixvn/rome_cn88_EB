using System.Collections.Generic;
using EB.Features.Main.Model;
using EB.Features.Site.Shared;
using Fanex.NAP.Core.Web.Session;
using MediatR;

namespace EB.Features.Site.Main.Requests
{
    public class MenuBarQuery : BaseQuery, IRequest<IEnumerable<MenuItem>>
    {
        public MenuBarQuery(ICurrentSession currentSession)
            : base(currentSession)
        {
        }
    }
}