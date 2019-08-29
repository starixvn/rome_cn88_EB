using EB.Features.Main.Params;
using EB.Features.Main.Resources;
using EB.Features.Main.Services;
using EB.Features.Shared.Resources;
using Fanex.Common;
using Fanex.NAP.Core.Models;
using Fanex.NAP.Core.Service.Interface;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Main.Tests.Services
{
    public class UserInfoServiceTest
    {
        private IUserService userService;
        private ISecurityService securityService;
        private IUserInfoService userInfoService;

        [SetUp]
        public void SetUp()
        {
            userService = Substitute.For<IUserService>();
            securityService = Substitute.For<ISecurityService>();
            userInfoService = new UserInfoService(userService, securityService);
        }

        [Test]
        public void GetUserInfo_Always_TrimUserName()
        {
            // Arrange
            var userName = " razor";

            // Act
            userInfoService.GetUserInfo(userName);

            // Assert
            userService.Received().Get(Arg.Is<string>(i => i == userName.Trim()));
        }

        [Test]
        public void GetUserInfo_Always_ReceivedCorrectFunction()
        {
            // Arrange
            var userId = 8;

            // Act
            userInfoService.GetUserInfo(userId);

            // Assert
            userService.Received().Get(Arg.Is<int>(i => i == userId));
        }

        [Test]
        public void ChangePassword_NoMatchOldPass_ReturnWrongOldPasssMessage()
        {
            // Arrange
            userService.Get(Arg.Any<int>()).Returns(new User { Password = "password" });
            var param = new ChangePasswordParam { OldPassword = "hashPassword1" };

            // Act
            var act = userInfoService.ChangePassword(param);

            // Assert
            Assert.AreEqual(MainResource.WrongOldPasswordMessage, act.Message);
        }

        [Test]
        public void ChangePassword_MatchOldPass_ReturnSuccessfullyMessage()
        {
            // Arrange
            userService.Get(Arg.Any<int>()).Returns(new User { Password = "password" });
            securityService.UpdatePassword(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns(true);
            var param = new ChangePasswordParam
            {
                OldPassword = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                NewPassword = "newpassword"
            };

            // Act
            var act = userInfoService.ChangePassword(param);

            // Assert
            Assert.AreEqual(MainResource.PasswordChangedSuccessfully, act.Message);
        }

        [Test]
        public void ChangePassword_UpdateFaildPass_ReturnErrorMessage()
        {
            // Arrange
            userService.Get(Arg.Any<int>()).Returns(new User { Password = "password" });
            securityService.UpdatePassword(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns(false);
            var param = new ChangePasswordParam
            {
                OldPassword = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                NewPassword = "newpassword"
            };

            // Act
            var act = userInfoService.ChangePassword(param);

            // Assert
            Assert.AreEqual(AppResources.SystemErrorMessage, act.Message);
        }

        [Test]
        public void ChangePassword_Always_NewPassMustBeEncryted()
        {
            // Arrange
            var expectedNewPass = Cryptography.CfsEnCode("newPassword");
            userService.Get(Arg.Any<int>()).Returns(new User { Password = "password" });
            securityService.UpdatePassword(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns(true);
            var param = new ChangePasswordParam
            {
                OldPassword = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                NewPassword = "newPassword" };

            // Act
            userInfoService.ChangePassword(param);

            // Assert
            securityService.Received().UpdatePassword(Arg.Any<int>(), Arg.Any<int>(), Arg.Is<string>(i => i == expectedNewPass));
        }
    }
}
