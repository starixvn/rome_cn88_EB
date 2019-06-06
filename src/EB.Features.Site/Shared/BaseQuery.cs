using System.Collections.Generic;
using Fanex.NAP.Core.Models;
using Fanex.NAP.Core.Web.Session;

namespace EB.Features.Site.Shared
{
    public class BaseQuery
    {
        public BaseQuery(ICurrentSession currentSession)
        {
            CurrentSession = currentSession;
        }

        public ICurrentSession CurrentSession { get; }

        public IList<MenuItem> MenuList => CurrentSession.MenuItemList;

        public virtual bool IsValid() => true;
    }
}
