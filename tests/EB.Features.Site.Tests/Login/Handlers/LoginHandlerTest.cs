using EB.Features.Main.Services;
using EB.Features.Site.Login.Handlers;
using EB.Features.Site.Login.Requests;
using EB.Features.Site.Shared.Models;
using Fanex.NAP.Core.Models;
using Fanex.NAP.Core.Models.ParameterObject;
using Fanex.NAP.Core.Service.Interface;
using Fanex.NAP.Core.Web.Context;
using Fanex.NAP.Core.Web.Session;
using NSubstitute;
using NUnit.Framework;

namespace EB.Features.Site.Tests.Login.Handlers
{
    public class LoginHandlerTest
    {
        private IAuthService authService;
        private IUserInfoService userInfoService;
        private ICurrentContext napClientContext;
        private ISecurityService securityService;
        private UserSession userSession;
        private ICurrentSession napClientSession;
        private LoginHandler handler;

        [SetUp]
        public void SetUp()
        {
            userInfoService = Substitute.For<IUserInfoService>();
            napClientContext = Substitute.For<ICurrentContext>();
            securityService = Substitute.For<ISecurityService>();
            userSession = Substitute.For<UserSession>();
            authService = Substitute.For<IAuthService>();
            napClientSession = Substitute.For<ICurrentSession>();

            handler = new LoginHandler(authService, userInfoService, securityService, napClientContext, napClientSession, userSession);
        }


        [Test]
        public void ValidateLogin_Fail_ReturnErrorMessage()
        {
            // Arrange
            var expectedError = "Incorrect user name";
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(expectedError);

            // Act
            var act = handler.Handle(new LoginQuery());

            // Assert
            Assert.AreEqual(expectedError, act.ErrorMessage);
        }

        [Test]
        public void ValidateLogin_Fail_ReturnEmptyUser()
        {
            // Arrange            
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns("Incorrect user name");

            // Act
            var act = handler.Handle(new LoginQuery());

            // Assert
            Assert.AreEqual(0, act.UserInfo.Id);
            Assert.IsTrue(string.IsNullOrEmpty(act.UserInfo.Username));
        }

        [Test]
        public void ValidateLogin_Fail_ReturnCorrectLanguageCode()
        {
            // Arrange
            var expectedLanguageCode = "en-US";
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns("Incorrect user name");

            // Act
            var act = handler.Handle(new LoginQuery { HideLanguage = expectedLanguageCode });

            // Assert
            Assert.AreEqual(expectedLanguageCode, act.LanguageCode);
        }

        [Test]
        public void ValidateLogin_Fail_NotCallGetUserInfo()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns("Incorrect user name");

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            userInfoService.DidNotReceive().GetUserInfo(Arg.Any<string>());
        }


        [Test]
        public void ValidateLogin_Success_CallGetUserInfo()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User());

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            userInfoService.Received().GetUserInfo(Arg.Any<string>());
        }


        [Test]
        public void ValidateLogin_Success_CallLoginOfSecurityService()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User());

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            securityService.Received().Login(Arg.Any<LoginParameter>());
        }

        [Test]
        public void ValidateLogin_Success_SetUserIdToSession()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User { Id = 1 });

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            userSession.Received().UserId = 1;
        }

        [Test]
        public void ValidateLogin_Success_SetUserNameToSession()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User { Username = "razor" });

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            userSession.Received().UserName = "razor";
        }

        [Test]
        public void ValidateLogin_Success_SetOnUserIdToSession()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User());
            securityService.Login(Arg.Any<LoginParameter>()).Returns(1);

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            userSession.Received().OnUserId = 1;
        }

        [Test]
        public void ValidateLogin_Success_SetSessionIdToSession()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User());

            // Act
            handler.Handle(new LoginQuery { SessionId = "sessionid" });

            // Assert
            userSession.Received().SessionId = "sessionid";
        }

        [Test]
        public void ValidateLogin_Success_SetLanguageToSession()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User());

            // Act
            handler.Handle(new LoginQuery { HideLanguage = "abc" });

            // Assert
            userSession.Received().Language = "abc";
        }

        [Test]
        public void ValidateLogin_Success_SetUserNameToNapClientSession()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User { Username = "razor" });

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            napClientSession.Received().Username = "razor";
        }

        [Test]
        public void ValidateLogin_Success_SetUserIdToNapClientSession()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User { Id = 123 });

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            napClientSession.Received().UserId = 123;
        }
    }
}
