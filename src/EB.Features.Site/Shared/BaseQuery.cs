namespace EB.Features.Site.Shared
{
    using EB.Features.Site.Shared.Models;

    public class BaseQuery
    {
        public BaseQuery(UserSession currentSession)
        {
            CurrentSession = currentSession;
        }

        public UserSession CurrentSession { get; }

        public virtual bool IsValid() => true;
    }
}
