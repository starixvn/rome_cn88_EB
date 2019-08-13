using System.Collections.Generic;
using EB.Features.Main.Resources;
using EB.Features.Main.Services;
using Fanex.NAP.Core.Models;
using Fanex.NAP.Core.Models.ValueObject;
using Fanex.NAP.Core.Service.Interface;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Main.Tests.Services
{
    public class AuthServiceTest
    {
        private IUserService userService;
        private IGroupService groupService;
        private IAuthService authService;

        [SetUp]
        public void SetUp()
        {
            userService = Substitute.For<IUserService>();
            groupService = Substitute.For<IGroupService>();
            authService = new AuthService(userService, groupService);
        }


        [Test]
        public void Login_EmptyUserName_ReturnInvalidInput()
        {
            // Arrange


            // Act
            var act = authService.ValidateLogin(string.Empty, "123");

            // Assert
            Assert.AreEqual(MainResource.Invalidinput, act);
        }


        [Test]
        public void Login_EmptyPassword_ReturnInvalidInput()
        {
            // Arrange


            // Act
            var act = authService.ValidateLogin("razor", string.Empty);

            // Assert
            Assert.AreEqual(MainResource.Invalidinput, act);
        }


        [Test]
        public void Login_TrimUserName_WhenCallNapAPI()
        {
            // Arrange


            // Act
            authService.ValidateLogin(" razor", "123");

            // Assert
            userService.Received().Get(Arg.Is<string>(i => i == "razor"));
        }


        [Test]
        public void Login_NotExistUserName_ReturnInvalidUserName()
        {
            // Arrange
            User user = null;
            userService.Get(Arg.Any<string>()).Returns(user);

            // Act
            var act = authService.ValidateLogin("razor", "123");

            // Assert
            Assert.AreEqual(MainResource.Invalid_Username_or_Password, act);
        }

        [Test]
        public void Login_IncorrectPass_ReturnInvalidUserName()
        {
            // Arrange
            var user = new User { Password="password" };

            userService.Get(Arg.Any<string>()).Returns(user);

            // Act
            var act = authService.ValidateLogin("razor", "123");

            // Assert
            Assert.AreEqual(MainResource.Invalid_Username_or_Password, act);
        }

        [Test]
        public void Login_NoPermissionOnNAP_ReturnNoAuthorization()
        {
            // Arrange
            var user = new User { Password = "newpassword" };

            userService.Get(Arg.Any<string>()).Returns(user);

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb");

            // Assert
            Assert.AreEqual(MainResource.You_have_no_authorization, act);
        }

        [Test]
        public void Login_GroupIsNull_ReturnNoAuthorization()
        {
            // Arrange
            var user = new User { Password = "newpassword" };

            userService.Get(Arg.Any<string>()).Returns(user);

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb");

            // Assert
            Assert.AreEqual(MainResource.You_have_no_authorization, act);
        }

        [Test]
        public void Login_SiteLevelIsInactice_ReturnNoAuthorization()
        {
            // Arrange
            var user = new User { Password = "newpassword" };
            userService.Get(Arg.Any<string>()).Returns(user);
            userService.GetSiteLevel(Arg.Any<int>(), Arg.Any<string>())
                .Returns(new List<SiteLevel>
                {
                    new SiteLevel { IsActive = false, Site = new Site{ Id = 67 }  }
                });

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb");

            // Assert
            Assert.AreEqual(MainResource.You_have_no_authorization, act);
        }

        [Test]
        public void Login_NoFindMenu_ReturnNoAuthorization()
        {
            // Arrange
            var user = new User { Password = "newpassword" };
            userService.Get(Arg.Any<string>()).Returns(user);
            userService.GetSiteLevel(Arg.Any<int>(), Arg.Any<string>())
                .Returns(new List<SiteLevel>
                {
                    new SiteLevel { IsActive = true, Site = new Site{ Id = 67 }  }
                });
            userService.GetMenuItem(Arg.Any<int>(), Arg.Any<int>()).Returns(new List<MenuItem>());

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb");

            // Assert
            Assert.AreEqual(MainResource.You_have_no_authorization, act);
        }

        [Test]
        public void Login_AllIsOk_ReturnEmptyMessage()
        {
            // Arrange
            var user = new User { Password = "newpassword" };
            userService.Get(Arg.Any<string>()).Returns(user);
            userService.GetSiteLevel(Arg.Any<int>(), Arg.Any<string>())
                .Returns(new List<SiteLevel>
                {
                    new SiteLevel { IsActive = true, Site = new Site{ Id = 67 } }
                });
            userService.GetMenuItem(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new List<MenuItem> {
                    new MenuItem()
                });

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb");

            // Assert
            Assert.AreEqual(string.Empty, act);
        }
    }
}
