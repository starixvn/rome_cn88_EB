using Fanex.NAP.Core.Web.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EB.Features.Site.Main.ViewModel
{
    public class IndexViewModel
    {
        private readonly ICurrentSession currentSession;

        public IndexViewModel(ICurrentSession currentSession)
        {
            this.currentSession = currentSession;
        }

        public string MainRootPath => ("~/Main");

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
    }
}