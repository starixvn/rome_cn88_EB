namespace EB.Features.Site.Main.Requests
{
    using System.Collections.Generic;
    using EB.Features.Main.Model;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Models;
    using MediatR;

    public class MenuBarQuery : BaseQuery, IRequest<IEnumerable<MenuItem>>
    {
        public MenuBarQuery(UserSession currentSession)
            : base(currentSession)
        {
        }
    }
}