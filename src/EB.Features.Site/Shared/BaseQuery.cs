namespace EB.Features.Site.Shared
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Web.Session;

    public class BaseQuery
    {
        public BaseQuery(ICurrentSession currentSession)
        {
            CurrentSession = currentSession;
        }

        public ICurrentSession CurrentSession { get; }

        public virtual bool IsValid() => true;
    }
}
