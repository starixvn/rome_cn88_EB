using Fanex.NAP.Core.Models;
using Fanex.NAP.Core.Web.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EB.Features.Site.Shared
{
    public class BaseQuery
    {
        public BaseQuery(ICurrentSession currentSession)
        {
            this.currentSession = currentSession;
        }

        public ICurrentSession currentSession { get; }

        public IList<MenuItem> MenuList => currentSession.MenuItemList;

        public virtual bool IsValid() => true;
    }
}