using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EB.Features.Main.Services;
using EB.Features.Site.Main.Handlers;
using EB.Features.Site.Main.Requests;
using EB.Features.Site.Main.ViewModel;
using EB.Features.Site.Shared.Helpers;
using EB.Features.Site.Shared.Models;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Main.Handlers
{
    public class MainHandlerTest
    {
        private IMenuService siteMenuService;
        private IBrowserHelper browserHelper;
        private Func<string, string> encodeDataFunction;
        private MainHandler mainHandler;

        [SetUp]
        public void SetUp()
        {
            siteMenuService = Substitute.For<IMenuService>();
            browserHelper = Substitute.For<IBrowserHelper>();
            encodeDataFunction = Substitute.For<Func<string, string>>();
            mainHandler = new MainHandler(siteMenuService, browserHelper, encodeDataFunction);
        }


        [Test]
        public void HandlerMenuBarQuery_Always_CallGetMenuService()
        {
            // Arrange
            var userSession = Substitute.For<UserSession>();
            userSession.UserId = 123;
            var message = new MenuBarQuery(userSession);

            // Act
            mainHandler.Handle(message);

            // Assert
            siteMenuService.Received().GetMenu(Arg.Is<int>(i => i == userSession.UserId));
        }

        [Test]
        public void HandlerIndexQuery_Always_CallGetMenuService()
        {
            // Arrange
            var userSession = Substitute.For<UserSession>();
            var message = new IndexQuery(userSession);

            // Act
            var act = mainHandler.Handle(message);

            // Assert
            Assert.AreEqual(typeof(IndexViewModel), act.GetType());
        }
    }
}
