using System.Web;
using Alpha.Web.Helpers;
using EB.Features.Shared.Configuration;
using Fanex.NAP.Core.Web.Session;

namespace EB.Features.Site.Main.ViewModel
{
    public class IndexViewModel
    {
        private readonly ICurrentSession currentSession;

        public IndexViewModel(ICurrentSession currentSession)
        {
            this.currentSession = currentSession;
        }

        public string MainRootPath => HttpContext.Current.Response.ApplyAppPathModifier("~/");

        public double InitialScaleViewPort
        {
            get
            {
                //var browserName = browserHelper.GetDevice().Info.BrowserName.ToLower();
                //var userAgent = browserHelper.GetUserAgent().ToLower();
                //var isTablet = browserHelper.IsMobile(isOnlyTablet: true);

                //var isSmallScale = browserName.Contains("chrome") &&
                //    userAgent.Contains("android") &&
                //    !browserHelper.IsAndroidNativeBrowser(userAgent) &&
                //    !isTablet;

                //return isSmallScale ? 0.5 : 0.7;
                return 0.7;
            }
        }

        public string DefaultPath => HttpContext.Current.Response.ApplyAppPathModifier("/Home/Index");

        public string CustName => currentSession.Username;

        public static string GetMenuLink(string path)
            => HttpContext.Current.Response.ApplyAppPathModifier("~" + path);
    }
}