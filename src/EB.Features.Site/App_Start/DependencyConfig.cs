namespace EB.Features.Site.App_Start
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using EB.Features.Main.DataAccesses;
    using EB.Features.Main.Services;
    using EB.Features.Reports.Services;
    using EB.Features.Shared.DataAccesses;
    using EB.Features.Shared.Services;
    using EB.Features.Site.Shared;
    using EB.Features.Site.Shared.Helpers;
    using EB.Features.Site.Shared.Models;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Context;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.RemotingRef;
    using MediatR;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;

    public class DependencyConfig
    {
        public static void Init()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            RegisterMediator(container);
            RegisterNapConfiguration(container);
            RegisterMainModule(container);
            RegisterShareModule(container);
            RegisterReportModule(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void RegisterShareModule(Container container)
        {
            container.Register<IDynamicRepository, DynamicRepository>();
            container.Register<SportGroupDataAccess, SportGroupDataAccessImpl>();
            container.Register<IBettypeGroupService, BettypeGroupServiceImpl>();
            container.Register<ICacheRefService, CacheRefServiceImpl>();
            container.Register<IReportDateService, ReportDateServiceImpl>();
            container.Register<ISportGroupService, SportGroupServiceImpl>();
            container.RegisterSingleton<IBrowserHelper, BrowserHelperImpl>();

            container.RegisterSingleton<InternalLoginService, InternalLoginServiceImpl>();
            container.RegisterSingleton<InternalLoginDataAccess, InternalLoginDataAccessImpl>();
            container.Register<UserSession, UserSessionImpl>();
        }

        private static void RegisterReportModule(Container container)
        {
            container.Register<IHouseWinLossService, HouseWinLossService>();
            container.Register<IMatchWinLossService, MatchWinLossService>();
            container.Register<IMatchWinLossWorkbook, MatchWinLossWorkbookImpl>();
            container.Register<IHouseWinLossWorkbook, HouseWinLossWorkbookImpl>();
        }

        private static void RegisterMainModule(Container container)
        {
            container.Register<IMenuService, MenuService>();
            container.Register<IUnderMainternanceService, UnderMainternanceService>();
            container.Register<IAuthService, AuthService>();
            container.Register<IUserInfoService, UserInfoService>();
        }

        private static void RegisterMediator(Container container)
        {
            var assemblies = GetAssemblies();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);
            container.Register(typeof(ICancellableAsyncRequestHandler<>), assemblies);

            container.Collection.Register(typeof(INotificationHandler<>), assemblies);
            container.Collection.Register(typeof(IAsyncNotificationHandler<>), assemblies);
            container.Collection.Register(typeof(ICancellableAsyncNotificationHandler<>), assemblies);
            container.Collection.Register(typeof(IPipelineBehavior<,>), assemblies);

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(BaseController).Assembly;
        }

        private static void RegisterNapConfiguration(Container container)
        {
            container.Register<ICurrentContext, CurrentContext>();
            container.Register<IRemotingRefService, RemotingRefService>();
            container.Register<IUserService, UserService>();
            container.Register<IGroupService, GroupService>();
            container.Register<ISiteService, SiteService>();
            container.Register<ISecurityService, SecurityService>();
            container.Register<ICurrentSession, CurrentSession>();
            container.Register<IMaintenanceService, MaintenanceService>();
        }
    }
}
