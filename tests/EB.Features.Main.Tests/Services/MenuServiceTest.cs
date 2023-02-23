using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using EB.Features.Main.Services;
using Fanex.NAP.Core.Service.Interface;
using NSubstitute;
using NUnit.Framework;
using NapMenuItem = Fanex.NAP.Core.Models.MenuItem;

namespace EB.Features.Main.Tests.Services
{
    public class MenuServiceTest
    {
        private IUserService napClientUserService;
        private IMenuService userService;
        private Fixture fixture;

        [SetUp]
        public void SetUp()
        {
            napClientUserService = Substitute.For<IUserService>();
            userService = new MenuService(napClientUserService);
            fixture = new Fixture();
        }


        [Test]
        public void GetMenu_Always_CallNapClientApi()
        {
            // Arrange


            // Act
            userService.GetMenu(1);

            // Assert
            napClientUserService.Received().GetMenuItem(1, Arg.Any<int>(), Arg.Any<string>(), true);
        }


        [Test]
        public void GetMenu_ParentMenu_HasParentIsNull()
        {
            // Arrange
            var menu = new List<NapMenuItem>
            {
                new NapMenuItem { ParentId = 0 },
                new NapMenuItem(),
            };
            napClientUserService.GetMenuItem(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<bool>())
                .Returns(menu);

            // Act
            var act = userService.GetMenu(1);

            // Assert
            Assert.AreEqual(1, act.Count());
        }

        [Test]
        public void GetMenu_ParentMenu_HasOrderByDisplayOrder()
        {
            // Arrange
            var menu = new List<NapMenuItem>
            {
                new NapMenuItem { Id = 3, DisplayOrder = 3},
                new NapMenuItem { Id = 1, DisplayOrder = 1},
                new NapMenuItem { Id = 2, DisplayOrder = 2},
            };
            napClientUserService.GetMenuItem(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<bool>())
                .Returns(menu);

            // Act
            var act = userService.GetMenu(1).ToList();

            // Assert
            Assert.AreEqual("1", act[0].Key);
            Assert.AreEqual("2", act[1].Key);
            Assert.AreEqual("3", act[2].Key);
        }
    }
}
