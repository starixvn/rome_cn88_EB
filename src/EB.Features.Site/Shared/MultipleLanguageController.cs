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
        private readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture(Language.Default);

        protected override void Initialize(RequestContext requestContext)
        {
            var cultureName = UserSessionImpl.Current.Language;

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = Language.Default;
                UserSessionImpl.Current.Language = cultureName;
            }

            // Modify current thread's cultures
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;

            base.Initialize(requestContext);
        }
    }
}
