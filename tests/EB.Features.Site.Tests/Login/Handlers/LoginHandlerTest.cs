using EB.Features.Main.Resources;
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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(expectedError);

            // Act
            var act = handler.Handle(new LoginQuery());

            // Assert
            Assert.AreEqual(expectedError, act.ErrorMessage);
        }

        [Test]
        public void ValidateLogin_Fail_ReturnEmptyUser()
        {
            // Arrange            
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("Incorrect user name");

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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("Incorrect user name");

            // Act
            var act = handler.Handle(new LoginQuery { HideLanguage = expectedLanguageCode });

            // Assert
            Assert.AreEqual(expectedLanguageCode, act.LanguageCode);
        }

        [Test]
        public void ValidateLogin_Fail_NotCallGetUserInfo()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns("Incorrect user name");

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            userInfoService.DidNotReceive().GetUserInfo(Arg.Any<string>());
        }

        [Test]
        public void ValidateLogin_Success_CallGetUserInfo()
        {
            // Arrange
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
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
            authService.ValidateLogin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);
            userInfoService.GetUserInfo(Arg.Any<string>()).Returns(new User { Id = 123 });

            // Act
            handler.Handle(new LoginQuery());

            // Assert
            napClientSession.Received().UserId = 123;
        }

        [Test]
        public void ValidateSSOTime_Fail_ReturnsNoAuthoriazionMessage()
        {
            // Arrange
            var message = new SSOQuery
            {
                Time = "uxF3G7phU5cYyNad9MFWd2KjIdTPHbLx"
            };

            securityService.GetTime().Returns("8/29/2019 2:06:33");

            // Act
            var vm = handler.Handle(message);

            // Assert
            Assert.AreEqual(MainResource.You_have_no_authorization, vm.ErrorMessage);
        }

        [Test]
        public void ValidateSSOTime_Success_CallsSSOLogin()
        {
            // Arrange
            var message = new SSOQuery
            {
                UserId = "GbxI81VVtCZCnZCORy414g==",
                OnUserId = "A9Ut41LUDzQ=",
                Time = "uxF3G7phU5cYyNad9MFWd2KjIdTPHbLx",
                RedirectUrl = "~/SubAccount/Index",
                SessionId = "5jkkuqkbs0mrz3ed0xy2qji1",
                SiteId = 105
            };

            securityService.GetTime().Returns("8/28/2019 23:33:16 PM");

            napClientContext.IP.Returns("10.21.10.52");
            napClientContext.BrowserName.Returns("Chrome/76.0");
            napClientContext.CountryCode.Returns("244");
            napClientContext.DomainName.Returns("localhost");
            napClientContext.Platform.Returns("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.87 Safari/537.36");

            // Act
            handler.Handle(message);

            // Assert
            securityService.Received().SSOLogin(Arg.Any<SSOLoginParameter>());
        }

        [Test]
        public void SSOLogin_Fail_ReturnsNoAuthoriazionMessage()
        {
            // Arrange
            var message = new SSOQuery
            {
                UserId = "GbxI81VVtCZCnZCORy414g==",
                OnUserId = "A9Ut41LUDzQ=",
                Time = "uxF3G7phU5cYyNad9MFWd2KjIdTPHbLx",
                RedirectUrl = "~/SubAccount/Index",
                SessionId = "5jkkuqkbs0mrz3ed0xy2qji1",
                SiteId = 105
            };

            securityService.GetTime().Returns("8/28/2019 23:33:16 PM");

            napClientContext.IP.Returns("10.21.10.52");
            napClientContext.BrowserName.Returns("Chrome/76.0");
            napClientContext.CountryCode.Returns("244");
            napClientContext.DomainName.Returns("localhost");
            napClientContext.Platform.Returns("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.87 Safari/537.36");

            securityService.SSOLogin(Arg.Any<SSOLoginParameter>()).Returns(0);

            // Act
            var vm = handler.Handle(message);

            // Assert
            Assert.AreEqual(MainResource.You_have_no_authorization, vm.ErrorMessage);
        }

        [Test]
        public void SSOLogin_Success_CallsGetUserInfo()
        {
            // Arrange
            var message = new SSOQuery
            {
                UserId = "GbxI81VVtCZCnZCORy414g==",
                OnUserId = "A9Ut41LUDzQ=",
                Time = "uxF3G7phU5cYyNad9MFWd2KjIdTPHbLx",
                RedirectUrl = "~/SubAccount/Index",
                SessionId = "5jkkuqkbs0mrz3ed0xy2qji1",
                SiteId = 105
            };

            securityService.GetTime().Returns("8/28/2019 23:33:16 PM");

            napClientContext.IP.Returns("10.21.10.52");
            napClientContext.BrowserName.Returns("Chrome/76.0");
            napClientContext.CountryCode.Returns("244");
            napClientContext.DomainName.Returns("localhost");
            napClientContext.Platform.Returns("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.87 Safari/537.36");

            securityService.SSOLogin(Arg.Any<SSOLoginParameter>()).Returns(149882);

            // Act
            handler.Handle(message);

            // Assert
            userInfoService.Received().GetUserInfo(Arg.Is<int>(i => i == 38123688));
        }

        [Test]
        public void GetUserInfo_UserIsNull_ReturnsNoAuthoriazionMessage()
        {
            // Arrange
            var message = new SSOQuery
            {
                UserId = "GbxI81VVtCZCnZCORy414g==",
                OnUserId = "A9Ut41LUDzQ=",
                Time = "uxF3G7phU5cYyNad9MFWd2KjIdTPHbLx",
                RedirectUrl = "~/SubAccount/Index",
                SessionId = "5jkkuqkbs0mrz3ed0xy2qji1",
                SiteId = 105
            };

            securityService.GetTime().Returns("8/28/2019 23:33:16 PM");

            napClientContext.IP.Returns("10.21.10.52");
            napClientContext.BrowserName.Returns("Chrome/76.0");
            napClientContext.CountryCode.Returns("244");
            napClientContext.DomainName.Returns("localhost");
            napClientContext.Platform.Returns("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.87 Safari/537.36");

            securityService.SSOLogin(Arg.Any<SSOLoginParameter>()).Returns(149882);

            User user = null;
            userInfoService.GetUserInfo(38123688).Returns(user);

            // Act
            var vm = handler.Handle(message);

            // Assert
            Assert.AreEqual(MainResource.You_have_no_authorization, vm.ErrorMessage);
        }

        [Test]
        public void GetUserInfo_DontHaveMenus_ReturnsNoAuthoriazionMessage()
        {
            // Arrange
            var message = new SSOQuery
            {
                UserId = "GbxI81VVtCZCnZCORy414g==",
                OnUserId = "A9Ut41LUDzQ=",
                Time = "uxF3G7phU5cYyNad9MFWd2KjIdTPHbLx",
                RedirectUrl = "~/SubAccount/Index",
                SessionId = "5jkkuqkbs0mrz3ed0xy2qji1",
                SiteId = 105
            };

            securityService.GetTime().Returns("8/28/2019 23:33:16 PM");

            napClientContext.IP.Returns("10.21.10.52");
            napClientContext.BrowserName.Returns("Chrome/76.0");
            napClientContext.CountryCode.Returns("244");
            napClientContext.DomainName.Returns("localhost");
            napClientContext.Platform.Returns("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.87 Safari/537.36");

            securityService.SSOLogin(Arg.Any<SSOLoginParameter>()).Returns(149882);

            User user = new User();
            userInfoService.GetUserInfo(38123688).Returns(user);
            authService.CheckUserSitePermission(user).Returns(false);

            // Act
            var vm = handler.Handle(message);

            // Assert
            Assert.AreEqual(MainResource.You_have_no_authorization, vm.ErrorMessage);
        }

        [Test]
        public void GetUserInfo_Success_ReturnsCorrectViewModel()
        {
            // Arrange
            var message = new SSOQuery
            {
                UserId = "GbxI81VVtCZCnZCORy414g==",
                OnUserId = "A9Ut41LUDzQ=",
                Time = "uxF3G7phU5cYyNad9MFWd2KjIdTPHbLx",
                RedirectUrl = "~/SubAccount/Index",
                SessionId = "5jkkuqkbs0mrz3ed0xy2qji1",
                SiteId = 105
            };

            securityService.GetTime().Returns("8/28/2019 23:33:16 PM");

            napClientContext.IP.Returns("10.21.10.52");
            napClientContext.BrowserName.Returns("Chrome/76.0");
            napClientContext.CountryCode.Returns("244");
            napClientContext.DomainName.Returns("localhost");
            napClientContext.Platform.Returns("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.87 Safari/537.36");

            securityService.SSOLogin(Arg.Any<SSOLoginParameter>()).Returns(149882);

            User user = new User { Id = 38123688, Name = "kanrome" };
            userInfoService.GetUserInfo(38123688).Returns(user);
            authService.CheckUserSitePermission(user).Returns(true);

            // Act
            var vm = handler.Handle(message);

            // Assert
            Assert.AreEqual(string.Empty, vm.ErrorMessage);
        }
    }
}
