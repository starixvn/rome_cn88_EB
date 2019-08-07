namespace EB.Features.Site.Shared.Models
{
    using System.Web;
    using EB.Features.Shared.Constants;
    using Fanex.Common;

    public interface UserSession
    {
        string Language { get; set; }
        string UserName { get; set; }
        int UserId { get; set; }
        int OnUserId { get; set; }
        string SessionId { get; set; }
        string Ip { get; }
    }

    public class UserSessionImpl : UserSession
    {
        public const string DEFAULT_LANGUAGE = "zh-Hans";
        private readonly string ApplicationName = "EB";

        public static UserSession Current
            => new UserSessionImpl() as UserSession;

        public string Language
        {
            get
            {
                object value = GetProperty("LANGUAGE");
                return (null == value) ? DEFAULT_LANGUAGE : value as string;
            }

            set
            {
                var languageCode = LanguageConstants.Standardize(value);
                SetProperty("LANGUAGE", languageCode);
            }
        }

        public string UserName
        {
            get
            {
                return GetProperty("USERNAME") as string;
            }

            set
            {
                SetProperty("USERNAME", value);
            }
        }

        public int UserId
        {
            get
            {
                var value = GetProperty("USERID");
                return Converter.ToInt(value);
            }

            set
            {
                SetProperty("USERID", value);
            }
        }
        public int OnUserId
        {
            get
            {
                var value = GetProperty("ONUSERID");
                return Converter.ToInt(value);
            }

            set
            {
                SetProperty("ONUSERID", value);
            }
        }

        public string SessionId
        {
            get
            {
                return GetProperty("SESSIONID") as string;
            }

            set
            {
                SetProperty("SESSIONID", value);
            }
        }

        public string Ip
        {
            get
            {
                var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                return string.IsNullOrEmpty(ip) || ip.Equals("::1") ? "127.0.0.1" : ip.Split(',')[0];
            }
        }
        private object GetProperty(string propertyName)
            => HttpContext.Current.Session[$"{ApplicationName}-{propertyName}"];

        public void SetProperty(string propertyName, object value)
        {
            HttpContext.Current.Session[$"{ApplicationName}-{propertyName}"] = value;
        }
    }
}
