using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Alpha.Logging;
using EB.Features.Shared.Configuration;
using EB.Features.Site.App_Start;
using EB.Features.Site.Shared;
using EB.Features.Site.Shared.Models;
using Fanex.Data;
using Fanex.Data.CrossCutting.WrappingException;
using Fanex.Data.OldConnectionStringProvider;
using Fanex.Logging.Extensions.Database;
using Fanex.Logging.Extensions.Web;
using Fanex.RemotingRef;
using ONEbook.UM.Filters;

namespace EB.Features.Site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var sensitivteInfo = new[] { "password", "seccode", "securitycode" };
            LoggingConfig.Init(sensitivteInfo, DatabaseInfoGetters.GetDbExceptionInfo);

            DependencyConfig.Init();
            ModelBinders.Binders.DefaultBinder = new QueryEntityModelBinder();

            DbSettingProviderManager
                .StartNewSession()
                .Use(new OldConnectionStringProvider())
                .UseDefaultDbSettingProvider(resourcePath: AppSettings.StoredProcedurePath)
                .Use((settingKey, commandTimeout) => new WrappingExceptionObjectDb(settingKey, commandTimeout))
                .Run();

            GlobalFilters.Filters.Add(new CheckUMFilter());

            StaticResources.Init(GetBuildVersion());

            RefDataTypeContainer
                .ConnectTo(new Uri(AppSettings.RemoteRefURL))
                .FromApplication("EB.Features.Site");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Request.IsLocal)
            {
                return;
            }

            var ex = Server.GetLastError();
            var user = UserSessionImpl.Current;

            ExceptionHandler.Handle(ex,
                new Dictionary<string, object>
                {
                    ["UserId"] = user?.UserId,
                    ["UserName"] = user?.UserName,
                    ["Language"] = user?.Language
                });
        }

        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
        }
    }
}