namespace EB.Features.Site.Shared.Models
{
    using System.Web;

    public class UserSessionImpl : UserSession
    {
        public const string DEFAULT_LANGUAGE = "zh-Hans";
        private readonly string netSessionId;

        public UserSessionImpl()
        {
        }

        protected UserSessionImpl(string netSessionId)
        {
            this.netSessionId = netSessionId;
        }

        public static UserSession GetUserSessionInstance(string netSessionId)
            => new UserSessionImpl(netSessionId) as UserSession;

        public static UserSession Current
            => GetUserSessionInstance(HttpContext.Current.Session.SessionID);

        public string Language
        {
            get
            {
                object value = GetProperty("LANGUAGE");
                return (null == value) ? DEFAULT_LANGUAGE : value as string;
            }

            set
            {
                SetProperty("LANGUAGE", value);
            }
        }

        private object GetProperty(string propertyName)
            => HttpContext.Current.Session[$"{netSessionId}-{propertyName}"];

        public void SetProperty(string propertyName, object value)
        {
            HttpContext.Current.Session[$"{netSessionId}-{propertyName}"] = value;
        }
    }
}
