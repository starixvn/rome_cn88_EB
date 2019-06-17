namespace EB.Features.Site.Shared
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;
    using EB.Features.Shared.Constants;
    using EB.Features.Site.Shared.Models;

    public class MultipleLanguageController : Controller
    {
        private readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture(LanguageConstants.Default);

        protected override void Initialize(RequestContext requestContext)
        {
            var languageCode = requestContext.HttpContext.Request["HideLanguage"];
            languageCode = string.IsNullOrEmpty(languageCode) ? UserSessionImpl.Current.Language : languageCode;
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = LanguageConstants.Default;
            }

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;

            base.Initialize(requestContext);
        }
    }
}
