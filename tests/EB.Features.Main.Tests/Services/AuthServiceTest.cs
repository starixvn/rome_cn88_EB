using System.Collections.Generic;
using EB.Features.Main.Criteria.InternalLogin;
using EB.Features.Main.Resources;
using EB.Features.Main.Services;
using Fanex.Data.Repository;
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
        private IDynamicRepository dynamicRepository;

        [SetUp]
        public void SetUp()
        {
            userService = Substitute.For<IUserService>();
            groupService = Substitute.For<IGroupService>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            authService = new AuthService(userService, groupService, dynamicRepository);
        }


        [Test]
        public void Login_EmptyUserName_ReturnInvalidInput()
        {
            // Arrange


            // Act
            var act = authService.ValidateLogin(string.Empty, "123","192.168.1.1");

            // Assert
            Assert.AreEqual(MainResource.Invalidinput, act);
        }


        [Test]
        public void Login_EmptyPassword_ReturnInvalidInput()
        {
            // Arrange


            // Act
            var act = authService.ValidateLogin("razor", string.Empty, "192.168.1.1");

            // Assert
            Assert.AreEqual(MainResource.Invalidinput, act);
        }


        [Test]
        public void Login_TrimUserName_WhenCallNapAPI()
        {
            // Arrange
            dynamicRepository.When(x => x.Execute(Arg.Any<CheckIPOfficeCommand>()))
                .Do(x => ((CheckIPOfficeCommand)x.Args()[0]).IsAccess = true);

            // Act
            authService.ValidateLogin(" razor", "123","192.168.1.1");

            // Assert
            userService.Received().Get(Arg.Is<string>(i => i == "razor"));
        }


        [Test]
        public void Login_NotExistUserName_ReturnInvalidUserName()
        {
            // Arrange
            User user = null;
            userService.Get(Arg.Any<string>()).Returns(user);
            dynamicRepository.When(x => x.Execute(Arg.Any<CheckIPOfficeCommand>()))
                .Do(x => ((CheckIPOfficeCommand)x.Args()[0]).IsAccess = true);

            // Act
            var act = authService.ValidateLogin("razor", "123", "192.168.1.1");
            
            // Assert
            Assert.AreEqual(MainResource.Invalid_Username_or_Password, act);
        }

        [Test]
        public void Login_IncorrectPass_ReturnInvalidUserName()
        {
            // Arrange
            var user = new User { Password="password" };

            userService.Get(Arg.Any<string>()).Returns(user);
            dynamicRepository.When(x => x.Execute(Arg.Any<CheckIPOfficeCommand>()))
                .Do(x => ((CheckIPOfficeCommand)x.Args()[0]).IsAccess = true);

            // Act
            var act = authService.ValidateLogin("razor", "123", "192.168.1.1");

            // Assert
            Assert.AreEqual(MainResource.Invalid_Username_or_Password, act);
        }

        [Test]
        public void Login_NoPermissionOnNAP_ReturnNoAuthorization()
        {
            // Arrange
            var user = new User { Password = "newpassword" };

            userService.Get(Arg.Any<string>()).Returns(user);
            dynamicRepository.When(x => x.Execute(Arg.Any<CheckIPOfficeCommand>()))
                .Do(x => ((CheckIPOfficeCommand)x.Args()[0]).IsAccess = true);

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb", "192.168.1.1");

            // Assert
            Assert.AreEqual(MainResource.You_have_no_authorization, act);
        }

        [Test]
        public void Login_GroupIsNull_ReturnNoAuthorization()
        {
            // Arrange
            var user = new User { Password = "newpassword" };

            userService.Get(Arg.Any<string>()).Returns(user);
            dynamicRepository.When(x => x.Execute(Arg.Any<CheckIPOfficeCommand>()))
                .Do(x => ((CheckIPOfficeCommand)x.Args()[0]).IsAccess = true);

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb", "192.168.1.1");

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
                    new SiteLevel { IsActive = false, SiteId = 67  }
                });
            dynamicRepository.When(x => x.Execute(Arg.Any<CheckIPOfficeCommand>()))
                .Do(x => ((CheckIPOfficeCommand)x.Args()[0]).IsAccess = true);

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb", "192.168.1.1");

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
                    new SiteLevel { IsActive = true, SiteId = 67  }
                });
            userService.GetMenuItem(Arg.Any<int>(), Arg.Any<int>()).Returns(new List<MenuItem>());
            dynamicRepository.When(x => x.Execute(Arg.Any<CheckIPOfficeCommand>()))
                .Do(x => ((CheckIPOfficeCommand)x.Args()[0]).IsAccess = true);

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb", "192.168.1.1");

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
                    new SiteLevel { IsActive = true, SiteId = 67 }
                });
            userService.GetMenuItem(Arg.Any<int>(), Arg.Any<int>())
                .Returns(new List<MenuItem> {
                    new MenuItem()
                });
            dynamicRepository.When(x => x.Execute(Arg.Any<CheckIPOfficeCommand>()))
                .Do(x => ((CheckIPOfficeCommand)x.Args()[0]).IsAccess = true);

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb", "192.168.1.1");

            // Assert
            Assert.AreEqual(string.Empty, act);
        }


        [Test]
        public void CheckIPOffice_SpecialUserAccessFromNotOffice_ReturInvalidInput()
        {
            // Arrange
            dynamicRepository.When(x => x.Execute(Arg.Any<CheckIPOfficeCommand>()))
                .Do(x => ((CheckIPOfficeCommand)x.Args()[0]).IsAccess = false);

            // Act
            var act = authService.ValidateLogin("razor", "089542505d659cecbb988bb5ccff5bccf85be2dfa8c221359079aee2531298bb", "192.168.1.1");

            // Assert
            Assert.AreEqual(MainResource.Invalidinput, act);
        }
    }
}
