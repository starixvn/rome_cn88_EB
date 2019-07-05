namespace EB.Features.Site.Main.ViewModel
{
    using System;
    using System.Web;
    using EB.Features.Site.Main.Requests;
    using EB.Features.Site.Shared.Helpers;
    using EB.Features.Site.Shared.Models;

    public class IndexViewModel
    {
        private readonly UserSession currentSession;
        private readonly IBrowserHelper browserHelper;
        private readonly Func<string, string> encodeDataFunction;
        private readonly IndexQuery query;

        public IndexViewModel(
            UserSession currentSession,
            IBrowserHelper browserHelper,
            Func<string, string> encodeDataFunction,
            IndexQuery query)
        {
            this.currentSession = currentSession;
            this.browserHelper = browserHelper;
            this.encodeDataFunction = encodeDataFunction;
            this.query = query;
        }

        public static string MainRootPath => HttpContext.Current.Response.ApplyAppPathModifier("~/");

        public string CurrentURLCookieName => CookieHelper.GetCookieNameOfCurrentPage(CustId, 0, encodeDataFunction);

        public double InitialScaleViewPort
        {
            get
            {
                var browserName = browserHelper.GetDevice().Info.BrowserName.ToLowerInvariant();
                var userAgent = browserHelper.GetUserAgent().ToLowerInvariant();
                var isTablet = browserHelper.IsMobile(isOnlyTablet: true);

                var isSmallScale = browserName.Contains("chrome") &&
                    userAgent.Contains("android") &&
                    !browserHelper.IsAndroidNativeBrowser(userAgent) &&
                    !isTablet;

                return isSmallScale ? 0.5 : 0.7;
            }
        }

        public static string DefaultPath => HttpContext.Current.Response.ApplyAppPathModifier("/Home/Index");

        public string CustName => currentSession.UserName;

        public int CustId => query.CurrentSession.OnUserId;

        public static string GetMenuLink(string path)
            => HttpContext.Current.Response.ApplyAppPathModifier("~" + path);
    }
}