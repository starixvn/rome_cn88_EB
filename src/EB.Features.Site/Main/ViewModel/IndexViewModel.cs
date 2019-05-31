using System;
using System.Web;
using EB.Features.Site.Main.Requests;
using EB.Features.Site.Shared.Helpers;
using Fanex.NAP.Core.Web.Session;

namespace EB.Features.Site.Main.ViewModel
{
    public class IndexViewModel
    {
        private readonly ICurrentSession currentSession;
        private readonly BrowserHelper browserHelper;
        private readonly Func<string, string> encodeDataFunction;
        private readonly IndexQuery query;

        public IndexViewModel(
            ICurrentSession currentSession,
            BrowserHelper browserHelper,
            Func<string, string> encodeDataFunction,
            IndexQuery query)
        {
            this.currentSession = currentSession;
            this.browserHelper = browserHelper;
            this.encodeDataFunction = encodeDataFunction;
            this.query = query;
        }

        public string MainRootPath => HttpContext.Current.Response.ApplyAppPathModifier("~/");

        public string CurrentURLCookieName => CookieHelper.GetCookieNameOfCurrentPage(CustId, 0, encodeDataFunction);

        public double InitialScaleViewPort
        {
            get
            {
                var browserName = browserHelper.GetDevice().Info.BrowserName.ToLower();
                var userAgent = browserHelper.GetUserAgent().ToLower();
                var isTablet = browserHelper.IsMobile(isOnlyTablet: true);

                var isSmallScale = browserName.Contains("chrome") &&
                    userAgent.Contains("android") &&
                    !browserHelper.IsAndroidNativeBrowser(userAgent) &&
                    !isTablet;

                return isSmallScale ? 0.5 : 0.7;
            }
        }

        public string DefaultPath => HttpContext.Current.Response.ApplyAppPathModifier("/Home/Index");

        public string CustName => currentSession.Username;

        public int CustId => query.CurrentSession.UserId;

        public static string GetMenuLink(string path)
            => HttpContext.Current.Response.ApplyAppPathModifier("~" + path);
    }
}